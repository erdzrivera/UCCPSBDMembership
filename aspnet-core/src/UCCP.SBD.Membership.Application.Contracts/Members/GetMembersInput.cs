using Volo.Abp.Application.Dtos;

namespace UCCP.SBD.Membership.Members
{
    public class GetMembersInput : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public string? BirthdayStart { get; set; }
        public string? BirthdayEnd { get; set; }
    }
}
