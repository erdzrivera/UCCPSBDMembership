using System;
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

        public MemberAppService(IRepository<Member, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<MemberDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return MapToDto(entity);
        }

        public async Task<PagedResultDto<MemberDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var query = await _repository.GetQueryableAsync();
            
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

        public async Task<MemberDto> CreateAsync(CreateUpdateMemberDto input)
        {
            var entity = ObjectMapper.Map<CreateUpdateMemberDto, Member>(input);
            
            entity.MemberTypeId = MapMemberTypeToId(input.MemberTypeId);
            entity.OrganizationId = MapOrganizationToId(input.OrganizationId);
            
            await _repository.InsertAsync(entity);
            
            return MapToDto(entity);
        }

        public async Task<MemberDto> UpdateAsync(Guid id, CreateUpdateMemberDto input)
        {
            var entity = await _repository.GetAsync(id);
            
            ObjectMapper.Map(input, entity);
            
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
