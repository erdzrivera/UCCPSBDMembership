using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace UCCP.SBD.Membership.Members
{
    public interface IMemberAppService : IApplicationService
    {
        Task<MemberDto> GetAsync(Guid id);
        Task<PagedResultDto<MemberDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<MemberDto> CreateAsync(CreateUpdateMemberDto input);
        Task<MemberDto> UpdateAsync(Guid id, CreateUpdateMemberDto input);
        Task DeleteAsync(Guid id);
        // IAsyncCrudAppService inheritance removed to avoid resolution issues, manually defining methods matches implementation.
    }
}
