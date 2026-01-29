using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

using Microsoft.AspNetCore.Authorization;
using UCCP.SBD.Membership.Permissions;

namespace UCCP.SBD.Membership.Members
{
    [Authorize(MembershipPermissions.MembershipTypes.Default)]
    public class MembershipTypeAppService : CrudAppService<MembershipType, MembershipTypeDto, string, PagedAndSortedResultRequestDto, MembershipTypeDto>, IMembershipTypeAppService
    {
        public MembershipTypeAppService(IRepository<MembershipType, string> repository) : base(repository)
        {
            GetPolicyName = MembershipPermissions.MembershipTypes.Default;
            GetListPolicyName = MembershipPermissions.MembershipTypes.Default;
            CreatePolicyName = MembershipPermissions.MembershipTypes.Create;
            UpdatePolicyName = MembershipPermissions.MembershipTypes.Edit;
            DeletePolicyName = MembershipPermissions.MembershipTypes.Delete;
        }
    }
}
