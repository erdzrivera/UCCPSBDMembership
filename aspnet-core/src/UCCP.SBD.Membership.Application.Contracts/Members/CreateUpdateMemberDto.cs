using System.ComponentModel.DataAnnotations;

namespace UCCP.SBD.Membership.Members
{
    public class CreateUpdateMemberDto
    {
        [Required]
        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        public string Birthday { get; set; }
        public string Occupation { get; set; }
        public string BaptismDate { get; set; }
        public string BaptizedBy { get; set; }
        
        public string MemberTypeId { get; set; }
        public string OrganizationId { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
    }
}
