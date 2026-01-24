using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UCCP.SBD.Membership.Members
{
    public class OrganizationAppService : CrudAppService<Organization, OrganizationDto, string, PagedAndSortedResultRequestDto, OrganizationDto>, IOrganizationAppService
    {
        public OrganizationAppService(IRepository<Organization, string> repository) : base(repository)
        {
        }
    }
}
