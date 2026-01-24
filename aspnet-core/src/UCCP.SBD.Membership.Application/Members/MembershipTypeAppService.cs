using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UCCP.SBD.Membership.Members
{
    public class MembershipTypeAppService : CrudAppService<MembershipType, MembershipTypeDto, string, PagedAndSortedResultRequestDto, MembershipTypeDto>, IMembershipTypeAppService
    {
        public MembershipTypeAppService(IRepository<MembershipType, string> repository) : base(repository)
        {
        }
    }
}
