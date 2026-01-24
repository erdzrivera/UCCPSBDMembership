using Volo.Abp.Domain.Entities.Auditing;

namespace UCCP.SBD.Membership.Members
{
    public class Organization : FullAuditedAggregateRoot<string>
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        protected Organization() { }

        public Organization(string id, string name, string abbreviation) 
            : base(id)
        {
            Name = name;
            Abbreviation = abbreviation;
        }
    }
}
