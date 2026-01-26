using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace UCCP.SBD.Membership.EntityFrameworkCore;

[DependsOn(
    typeof(MembershipDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule)
)]
public class MembershipEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // Fix for PostgreSQL DateTime timezone issue with .NET 9
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        MembershipEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<MembershipDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also MembershipMigrationsDbContextFactory for EF Core tooling. */
            options.UseNpgsql();
        });
        
        // Fix for Render.com PostgreSQL connection string format (postgres://)
        context.Services.PostConfigure<Volo.Abp.Data.AbpDbConnectionOptions>(options =>
        {
            var connectionString = options.ConnectionStrings.Default;
            if (!string.IsNullOrEmpty(connectionString) && 
                (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://")))
            {
                var uri = new System.Uri(connectionString);
                var userInfo = uri.UserInfo.Split(':');
                var builder = new Npgsql.NpgsqlConnectionStringBuilder
                {
                    Host = uri.Host,
                    Port = uri.Port > 0 ? uri.Port : 5432,
                    Database = uri.AbsolutePath.TrimStart('/'),
                    Username = userInfo[0],
                    Password = userInfo.Length > 1 ? userInfo[1] : null,
                    SslMode = Npgsql.SslMode.Prefer // Render usually needs SSL
                };
                
                options.ConnectionStrings.Default = builder.ToString();
            }
        });
    }
}
