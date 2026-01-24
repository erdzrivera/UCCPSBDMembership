using Volo.Abp.Modularity;

namespace UCCP.SBD.Membership;

public abstract class MembershipApplicationTestBase<TStartupModule> : MembershipTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
