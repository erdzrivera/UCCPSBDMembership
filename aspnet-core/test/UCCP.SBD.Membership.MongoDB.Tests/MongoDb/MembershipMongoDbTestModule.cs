using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace UCCP.SBD.Membership.MongoDB;

[DependsOn(
    typeof(MembershipApplicationTestModule),
    typeof(MembershipMongoDbModule)
)]
public class MembershipMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MembershipMongoDbFixture.GetRandomConnectionString();
        });
    }
}
