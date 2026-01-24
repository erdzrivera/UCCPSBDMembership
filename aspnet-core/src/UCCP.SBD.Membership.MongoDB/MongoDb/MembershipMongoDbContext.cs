using UCCP.SBD.Membership.Members;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace UCCP.SBD.Membership.MongoDB;

[ConnectionStringName("Default")]
public class MembershipMongoDbContext : AbpMongoDbContext
{
    public IMongoCollection<Member> Members => Collection<Member>();
    public IMongoCollection<MembershipType> MembershipTypes => Collection<MembershipType>();
    public IMongoCollection<Organization> Organizations => Collection<Organization>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.Entity<Member>(b =>
        {
            b.CollectionName = "Members";
        });

        modelBuilder.Entity<MembershipType>(b =>
        {
            b.CollectionName = "MembershipTypes";
        });

        modelBuilder.Entity<Organization>(b =>
        {
            b.CollectionName = "Organizations";
        });
    }
}
