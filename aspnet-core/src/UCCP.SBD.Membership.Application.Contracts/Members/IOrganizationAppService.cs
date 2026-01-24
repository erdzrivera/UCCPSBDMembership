using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace UCCP.SBD.Membership.Members
{
    public interface IOrganizationAppService : ICrudAppService<OrganizationDto, string, PagedAndSortedResultRequestDto, OrganizationDto>
    {
    }
}
