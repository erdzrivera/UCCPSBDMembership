using UCCP.SBD.Membership.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace UCCP.SBD.Membership.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MembershipMongoDbModule),
    typeof(MembershipApplicationContractsModule),
    typeof(MembershipApplicationModule)
    )]
public class MembershipDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "Membership:"; });
    }
}
