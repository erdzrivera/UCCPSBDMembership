using System.Threading.Tasks;

namespace UCCP.SBD.Membership.Data;

public interface IMembershipDbSchemaMigrator
{
    Task MigrateAsync();
}
