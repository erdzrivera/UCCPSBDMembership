using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UCCP.SBD.Membership.Members
{
    public class MemberAppService : ApplicationService, IMemberAppService
    {
        private readonly IRepository<Member, Guid> _repository;
        private readonly Microsoft.Extensions.Logging.ILogger<MemberAppService> _logger;

        public MemberAppService(IRepository<Member, Guid> repository, Microsoft.Extensions.Logging.ILogger<MemberAppService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<MemberDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return MapToDto(entity);
        }

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

                if (!input.BirthdayStart.IsNullOrWhiteSpace())
                {
                    query = query.Where(x => x.Birthday.CompareTo(input.BirthdayStart) >= 0);
                }

                if (!input.BirthdayEnd.IsNullOrWhiteSpace())
                {
                    query = query.Where(x => x.Birthday.CompareTo(input.BirthdayEnd) <= 0);
                }

                var totalCount = await AsyncExecuter.CountAsync(query);

                query = query.OrderBy(input.Sorting ?? nameof(Member.CreationTime))
                             .PageBy(input);

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
}
