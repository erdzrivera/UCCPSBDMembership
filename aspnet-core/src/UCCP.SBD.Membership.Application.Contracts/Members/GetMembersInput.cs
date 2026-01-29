using Volo.Abp.Application.Dtos;

namespace UCCP.SBD.Membership.Members
{
    public class GetMembersInput : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public string? BirthdayStart { get; set; }
        public string? BirthdayEnd { get; set; }
        public string? OrganizationId { get; set; }
        public string? MemberTypeId { get; set; }
        public bool? IsActive { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
    }
}
