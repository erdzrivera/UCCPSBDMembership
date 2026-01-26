using UCCP.SBD.Membership.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace UCCP.SBD.Membership.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MembershipEntityFrameworkCoreModule),
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
