using Volo.Abp.Application.Dtos;

namespace UCCP.SBD.Membership.Members
{
    public class OrganizationDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}
