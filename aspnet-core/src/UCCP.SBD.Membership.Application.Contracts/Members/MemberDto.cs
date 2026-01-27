using System;
using Volo.Abp.Application.Dtos;

namespace UCCP.SBD.Membership.Members
{
    public class MemberDto : AuditedEntityDto<Guid>
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; }
        public string Occupation { get; set; }
        public string BaptismDate { get; set; }
        public string BaptizedBy { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? Sponsors { get; set; }
        public string MemberTypeId { get; set; }
        public string OrganizationId { get; set; }
        public bool IsActive { get; set; }
    }
}
