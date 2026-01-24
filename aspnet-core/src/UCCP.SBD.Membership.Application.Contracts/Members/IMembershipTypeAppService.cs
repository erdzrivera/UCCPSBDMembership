using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace UCCP.SBD.Membership.Members
{
    public interface IMembershipTypeAppService : ICrudAppService<MembershipTypeDto, string, PagedAndSortedResultRequestDto, MembershipTypeDto>
    {
    }
}
