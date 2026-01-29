using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace UCCP.SBD.Membership.Members
{
    public class Member : AuditedAggregateRoot<Guid>
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; }
        public string Occupation { get; set; }
        public string? BaptismDate { get; set; }
        public string? BaptizedBy { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? Sponsors { get; set; }
        
        public string MemberTypeId { get; set; }
        public string OrganizationId { get; set; }
        
        public bool IsActive { get; set; }

        protected Member() { }

        public Member(
            Guid id, 
            string firstName, 
            string lastName, 
            string? middleName, 
            string birthday, 
            string occupation, 
            string? baptismDate, 
            string? baptizedBy, 
            string? placeOfBirth,
            string? fatherName,
            string? motherName,
            string? sponsors,
            string memberTypeId, 
            string organizationId, 
            bool isActive) 
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Birthday = birthday;
            Occupation = occupation;
            BaptismDate = baptismDate;
            BaptizedBy = baptizedBy;
            PlaceOfBirth = placeOfBirth;
            FatherName = fatherName;
            MotherName = motherName;
            Sponsors = sponsors;
            MemberTypeId = memberTypeId;
            OrganizationId = organizationId;
            IsActive = isActive;
        }
    }
}
