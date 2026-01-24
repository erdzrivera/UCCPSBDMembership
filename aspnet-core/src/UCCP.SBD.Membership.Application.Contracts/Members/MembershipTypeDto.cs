using Volo.Abp.Application.Dtos;

namespace UCCP.SBD.Membership.Members
{
    public class MembershipTypeDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
