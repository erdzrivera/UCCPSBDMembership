using System.Threading.Tasks;
using UCCP.SBD.Membership.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using Microsoft.Extensions.Logging;

namespace UCCP.SBD.Membership
{
    public class MembershipDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IPermissionManager _permissionManager;
        private readonly Microsoft.Extensions.Logging.ILogger<MembershipDataSeedContributor> _logger;

        public MembershipDataSeedContributor(
            IPermissionManager permissionManager,
            Microsoft.Extensions.Logging.ILogger<MembershipDataSeedContributor> logger)
        {
            _permissionManager = permissionManager;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            _logger.LogInformation("---------- STARTING MEMBERSHIP SEEDING ----------");
            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.Members.Default, true);
            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.Organizations.Default, true);
            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.MembershipTypes.Default, true);
            
            // Sub-permissions
            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.Members.Create, true);
            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.Members.Edit, true);
            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.Members.Delete, true);

            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.Organizations.Create, true);
            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.Organizations.Edit, true);
            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.Organizations.Delete, true);

            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.MembershipTypes.Create, true);
            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.MembershipTypes.Edit, true);
            await _permissionManager.SetForRoleAsync("admin", MembershipPermissions.MembershipTypes.Delete, true);

            // Also seed for "Admin" (capitalized) just in case of case sensitivity issues
            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.Members.Default, true);
            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.Organizations.Default, true);
            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.MembershipTypes.Default, true);
            
            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.Members.Create, true);
            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.Members.Edit, true);
            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.Members.Delete, true);

            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.Organizations.Create, true);
            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.Organizations.Edit, true);
            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.Organizations.Delete, true);

            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.MembershipTypes.Create, true);
            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.MembershipTypes.Edit, true);
            await _permissionManager.SetForRoleAsync("Admin", MembershipPermissions.MembershipTypes.Delete, true);
            
            _logger.LogInformation("---------- FINISHED MEMBERSHIP SEEDING ----------");
        }
    }
}
