using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace UCCP.SBD.Membership.Members
{
    public class Member : AuditedAggregateRoot<Guid>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; }
        public string Occupation { get; set; }
        public string BaptismDate { get; set; }
        public string BaptizedBy { get; set; }
        
        public string MemberTypeId { get; set; }
        public string OrganizationId { get; set; }
        
        public bool IsActive { get; set; }

        protected Member() { }

        public Member(
            Guid id, 
            string firstName, 
            string lastName, 
            string middleName, 
            string birthday, 
            string occupation, 
            string baptismDate, 
            string baptizedBy, 
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
            MemberTypeId = memberTypeId;
            OrganizationId = organizationId;
            IsActive = isActive;
        }
    }
}
