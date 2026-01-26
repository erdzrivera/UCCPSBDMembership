using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UCCP.SBD.Membership.Localization;
using UCCP.SBD.Membership.EntityFrameworkCore;
using UCCP.SBD.Membership.MultiTenancy;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Uow;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.FeatureManagement;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace UCCP.SBD.Membership;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(MembershipEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
    )]
public class MembershipAuthServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("Membership");
                options.UseLocalServer();
                options.UseAspNetCore();
            });

            builder.AddServer(options =>
            {
                options.AllowAuthorizationCodeFlow();
                options.AllowPasswordFlow();
                options.AllowRefreshTokenFlow();
                options.AllowClientCredentialsFlow();
            });
        });

        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                // Use development certificates for cloud deployment
                // Production certificate file is not available in Docker container
                options.AddDevelopmentEncryptionAndSigningCertificate = true;
            });

            // Commented out - certificate file not available in Docker
            // PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
            // {
            //     serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", "9117282c-b3e1-4a9c-9963-a5f0105cb524");
            // });
        }
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        // ?? Swagger setup
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
            { "Membership", "Membership API" }
            },
            options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Membership API",
                    Version = "v1"
                });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });

        // ?? Localization setup
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<MembershipResource>()
                .AddBaseTypes(typeof(AbpUiResource), typeof(AccountResource));
        });

        // ?? Theme bundling setup
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(LeptonXLiteThemeBundles.Styles.Global, bundle =>
            {
                bundle.AddFiles("/global-styles.css");
            });
        });

        // ?? Auditing
        Configure<AbpAuditingOptions>(options =>
        {
            options.ApplicationName = "AuthServer";
            // options.IsEnabledForGetRequests = true;
        });

        // ?? VFS for dev
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<MembershipDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}UCCP.SBD.Membership.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<MembershipDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}UCCP.SBD.Membership.Domain"));
            });
        }

        // ?? App URLs
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());
            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });

        // ?? Background jobs
        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = false;
        });

        // ?? Caching
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "Membership:";
        });

        // ?? CORS policy
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        // ?? Dynamic claims
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });

        // Disable static background savers to avoid MongoDB transaction issues on standalone instances
        Configure<FeatureManagementOptions>(options =>
        {
            options.SaveStaticFeaturesToDatabase = false;
        });
        Configure<PermissionManagementOptions>(options =>
        {
            options.SaveStaticPermissionsToDatabase = false;
        });
        Configure<SettingManagementOptions>(options =>
        {
            options.SaveStaticSettingsToDatabase = false;
        });
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
 
        // Auto-create database schema on startup
        using (var scope = context.ServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<EntityFrameworkCore.MembershipDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            
            // Seed initial data
            await scope.ServiceProvider
                .GetRequiredService<Volo.Abp.Data.IDataSeeder>()
                .SeedAsync(new Volo.Abp.Data.DataSeedContext());
        }

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Membership API");
            options.OAuthClientId("Membership_Swagger"); // Must match seeded client_id
            options.OAuthClientSecret("Test1234");
            options.OAuthScopes("Membership");
            options.OAuthUsePkce(); // Required for public clients (no secret)
        });
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();

    }
}
