using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace UCCP.SBD.Membership.Data;

/* This is used if database provider does't define
 * IMembershipDbSchemaMigrator implementation.
 */
public class NullMembershipDbSchemaMigrator : IMembershipDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
