using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using UCCP.SBD.Membership.Members;

namespace UCCP.SBD.Membership.EntityFrameworkCore;

[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(IPermissionManagementDbContext))]
[ConnectionStringName("Default")]
public class MembershipDbContext :
    AbpDbContext<MembershipDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext,
    IPermissionManagementDbContext
{
    // Members
    public DbSet<Member> Members { get; set; }
    public DbSet<MembershipType> MembershipTypes { get; set; }
    public DbSet<Organization> Organizations { get; set; }

    // Identity
    public DbSet<Volo.Abp.Identity.IdentityUser> Users { get; set; }
    public DbSet<Volo.Abp.Identity.IdentityRole> Roles { get; set; }
    public DbSet<Volo.Abp.Identity.IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<Volo.Abp.Identity.OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<Volo.Abp.Identity.IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<Volo.Abp.Identity.IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<Volo.Abp.Identity.IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<Volo.Abp.Identity.IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Volo.Abp.TenantManagement.Tenant> Tenants { get; set; }
    public DbSet<Volo.Abp.TenantManagement.TenantConnectionString> TenantConnectionStrings { get; set; }

    // Permission Management
    public DbSet<Volo.Abp.PermissionManagement.PermissionGrant> PermissionGrants { get; set; }
    public DbSet<Volo.Abp.PermissionManagement.PermissionGroupDefinitionRecord> PermissionGroups { get; set; }
    public DbSet<Volo.Abp.PermissionManagement.PermissionDefinitionRecord> Permissions { get; set; }

    public MembershipDbContext(DbContextOptions<MembershipDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */
        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */
        builder.Entity<Member>(b =>
        {
            b.ToTable(MembershipConsts.DbTablePrefix + "Members", MembershipConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.FirstName).IsRequired().HasMaxLength(128);
            b.Property(x => x.MiddleName).IsRequired(false).HasMaxLength(128);
            b.Property(x => x.LastName).IsRequired().HasMaxLength(128);
            b.Property(x => x.Birthday).IsRequired();
            b.Property(x => x.Occupation).IsRequired().HasMaxLength(256);
            b.Property(x => x.BaptismDate).IsRequired(false);
            b.Property(x => x.BaptizedBy).IsRequired(false).HasMaxLength(256);
            b.Property(x => x.MemberTypeId).IsRequired();
            b.Property(x => x.OrganizationId).IsRequired();
            b.Property(x => x.PlaceOfBirth).IsRequired(false);
            b.Property(x => x.FatherName).IsRequired(false);
            b.Property(x => x.MotherName).IsRequired(false);
            b.Property(x => x.Sponsors).IsRequired(false);

            b.HasIndex(x => x.LastName);
        });

        builder.Entity<MembershipType>(b =>
        {
            b.ToTable(MembershipConsts.DbTablePrefix + "MembershipTypes", MembershipConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).IsRequired().HasMaxLength(512);

            b.HasIndex(x => x.Name);
        });

        builder.Entity<Organization>(b =>
        {
            b.ToTable(MembershipConsts.DbTablePrefix + "Organizations", MembershipConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.Abbreviation).IsRequired().HasMaxLength(64);

            b.HasIndex(x => x.Name);
        });
    }
}
