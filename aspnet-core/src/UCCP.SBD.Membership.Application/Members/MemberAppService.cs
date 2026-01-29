using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using UCCP.SBD.Membership.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

using System;

namespace UCCP.SBD.Membership.Members;

public class MemberAppService : ApplicationService, IMemberAppService
    {
        private readonly IRepository<Member, Guid> _repository;
        private readonly IRepository<MembershipType, string> _membershipTypeRepository;
        private readonly Microsoft.Extensions.Logging.ILogger<MemberAppService> _logger;

        public MemberAppService(
            IRepository<Member, Guid> repository, 
            IRepository<MembershipType, string> membershipTypeRepository,
            Microsoft.Extensions.Logging.ILogger<MemberAppService> logger)
        {
            _repository = repository;
            _membershipTypeRepository = membershipTypeRepository;
            _logger = logger;
        }

        [Authorize(MembershipPermissions.Members.Default)]
        public async Task<MemberDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return MapToDto(entity);
        }

        [Authorize(MembershipPermissions.Members.Default)]
        public async Task<PagedResultDto<MemberDto>> GetListAsync(GetMembersInput input)
        {
            try
            {
                var query = await _repository.GetQueryableAsync();
                
                if (!input.Filter.IsNullOrWhiteSpace())
                {
                    query = query.Where(x => x.FirstName.Contains(input.Filter) || 
                                             x.LastName.Contains(input.Filter) || 
                                             x.MiddleName.Contains(input.Filter));
                }

                if (!input.BirthdayStart.IsNullOrWhiteSpace() || !input.BirthdayEnd.IsNullOrWhiteSpace())
                {
                    // Normalize inputs to "MM-DD" (strip year "yyyy-" if present)
                    var startMd = !input.BirthdayStart.IsNullOrWhiteSpace() && input.BirthdayStart.Length > 5 
                        ? input.BirthdayStart.Substring(5) 
                        : input.BirthdayStart;
                        
                    var endMd = !input.BirthdayEnd.IsNullOrWhiteSpace() && input.BirthdayEnd.Length > 5 
                        ? input.BirthdayEnd.Substring(5) 
                        : input.BirthdayEnd;

                    if (!startMd.IsNullOrWhiteSpace() && !endMd.IsNullOrWhiteSpace())
                    {
                        // Wrap around case (e.g. Dec to Jan)
                        if (string.Compare(startMd, endMd) > 0)
                        {
                            query = query.Where(x => x.Birthday.Substring(5).CompareTo(startMd) >= 0 || 
                                                     x.Birthday.Substring(5).CompareTo(endMd) <= 0);
                        }
                        else
                        {
                            query = query.Where(x => x.Birthday.Substring(5).CompareTo(startMd) >= 0 && 
                                                     x.Birthday.Substring(5).CompareTo(endMd) <= 0);
                        }
                    }
                    else
                    {
                        if (!startMd.IsNullOrWhiteSpace())
                            query = query.Where(x => x.Birthday.Substring(5).CompareTo(startMd) >= 0);

                        if (!endMd.IsNullOrWhiteSpace())
                            query = query.Where(x => x.Birthday.Substring(5).CompareTo(endMd) <= 0);
                    }
                }

                var totalCount = await AsyncExecuter.CountAsync(query);

                if (input.Sorting?.Contains("memberTypeId", StringComparison.OrdinalIgnoreCase) == true)
                {
                    var isDesc = input.Sorting.Contains("desc", StringComparison.OrdinalIgnoreCase);
                    var membershipTypes = await _membershipTypeRepository.GetQueryableAsync();

                    var joinedQuery = from member in query
                                      join type in membershipTypes on member.MemberTypeId equals type.Id
                                      select new { Member = member, TypeName = type.Name };

                    if (isDesc)
                    {
                        joinedQuery = joinedQuery.OrderByDescending(x => x.TypeName);
                    }
                    else
                    {
                        joinedQuery = joinedQuery.OrderBy(x => x.TypeName);
                    }

                    // Select back the member and apply paging
                    query = joinedQuery.Select(x => x.Member);
                    query = query.PageBy(input);
                }
                else
                {
                    query = query.OrderBy(input.Sorting ?? nameof(Member.CreationTime))
                                 .PageBy(input);
                }

                var entities = await AsyncExecuter.ToListAsync(query);
                
                var dtos = new List<MemberDto>();
                foreach (var item in entities)
                {
                    dtos.Add(MapToDto(item));
                }
                
                return new PagedResultDto<MemberDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting members list: {Message}", ex.Message);
                throw; // Rethrow to let the user see the 500 error, but now we have logs
            }
        }

        [Authorize(MembershipPermissions.Members.Create)]
        public async Task<MemberDto> CreateAsync(CreateUpdateMemberDto input)
        {
            try 
            {
                Logger.LogInformation("Creating member: {FirstName} {LastName}", input.FirstName, input.LastName);
                
                var entity = ObjectMapper.Map<CreateUpdateMemberDto, Member>(input);
                entity.PlaceOfBirth = input.PlaceOfBirth;
                entity.FatherName = input.FatherName;
                entity.MotherName = input.MotherName;
                entity.Sponsors = input.Sponsors;
                
                Logger.LogInformation("Mapping MemberType: {MemberTypeId}", input.MemberTypeId);
                entity.MemberTypeId = MapMemberTypeToId(input.MemberTypeId);
                
                Logger.LogInformation("Mapping Organization: {OrganizationId}", input.OrganizationId);
                entity.OrganizationId = MapOrganizationToId(input.OrganizationId);
                
                await _repository.InsertAsync(entity);
                
                return MapToDto(entity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating member: {Message}", ex.Message);
                throw;
            }
        }

        [Authorize(MembershipPermissions.Members.Edit)]
        public async Task<MemberDto> UpdateAsync(Guid id, CreateUpdateMemberDto input)
        {
            var entity = await _repository.GetAsync(id);
            
            ObjectMapper.Map(input, entity);
            entity.PlaceOfBirth = input.PlaceOfBirth;
            entity.FatherName = input.FatherName;
            entity.MotherName = input.MotherName;
            entity.Sponsors = input.Sponsors;
            
            entity.MemberTypeId = MapMemberTypeToId(input.MemberTypeId);
            entity.OrganizationId = MapOrganizationToId(input.OrganizationId);
            
            await _repository.UpdateAsync(entity);
            
            return MapToDto(entity);
        }

        [Authorize(MembershipPermissions.Members.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        private MemberDto MapToDto(Member entity)
        {
            var dto = ObjectMapper.Map<Member, MemberDto>(entity);
            dto.PlaceOfBirth = entity.PlaceOfBirth;
            dto.FatherName = entity.FatherName;
            dto.MotherName = entity.MotherName;
            dto.Sponsors = entity.Sponsors;
            dto.MemberTypeId = MapIdToMemberType(entity.MemberTypeId);
            dto.OrganizationId = MapIdToOrganization(entity.OrganizationId);
            return dto;
        }

        private string MapMemberTypeToId(string type)
        {
            return type switch
            {
                "Regular" => "1",
                "Associate" => "2",
                "Affiliate" => "3",
                "Preparatory" => "4",
                "Honorary" => "5",
                _ => type 
            };
        }

        private string MapIdToMemberType(string id)
        {
            return id switch
            {
                "1" => "Regular",
                "2" => "Associate",
                "3" => "Affiliate",
                "4" => "Preparatory",
                "5" => "Honorary",
                _ => id
            };
        }
        
        private string MapOrganizationToId(string org)
        {
            return org?.ToUpper() switch
            {
                "UCSCA" => "1",
                "UCM" => "2",
                "CWA" => "3",
                "CYAF" => "4",
                "CYF" => "5",
                "KIDS" => "6",
                _ => org
            };
        }

        private string MapIdToOrganization(string id)
        {
            return id switch
            {
                "1" => "UCSCA",
                "2" => "UCM",
                "3" => "CWA",
                "4" => "CYAF",
                "5" => "CYF",
                "6" => "KIDS",
                _ => id
            };
        }
    }