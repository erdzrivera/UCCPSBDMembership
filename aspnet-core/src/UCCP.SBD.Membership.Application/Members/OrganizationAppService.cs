using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

using Microsoft.AspNetCore.Authorization;
using UCCP.SBD.Membership.Permissions;

namespace UCCP.SBD.Membership.Members
{
    [Authorize(MembershipPermissions.Organizations.Default)]
    public class OrganizationAppService : CrudAppService<Organization, OrganizationDto, string, PagedAndSortedResultRequestDto, OrganizationDto>, IOrganizationAppService
    {
        public OrganizationAppService(IRepository<Organization, string> repository) : base(repository)
        {
            GetPolicyName = MembershipPermissions.Organizations.Default;
            GetListPolicyName = MembershipPermissions.Organizations.Default;
            CreatePolicyName = MembershipPermissions.Organizations.Create;
            UpdatePolicyName = MembershipPermissions.Organizations.Edit;
            DeletePolicyName = MembershipPermissions.Organizations.Delete;
        }
    }
}
