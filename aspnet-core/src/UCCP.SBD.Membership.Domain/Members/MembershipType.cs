using Volo.Abp.Domain.Entities.Auditing;

namespace UCCP.SBD.Membership.Members
{
    public class MembershipType : FullAuditedAggregateRoot<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        protected MembershipType() { }

        public MembershipType(string id, string name, string description) 
            : base(id)
        {
            Name = name;
            Description = description;
        }
    }
}
