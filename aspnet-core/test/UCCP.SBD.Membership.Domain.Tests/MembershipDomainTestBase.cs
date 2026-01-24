using Volo.Abp.Modularity;

namespace UCCP.SBD.Membership;

/* Inherit from this class for your domain layer tests. */
public abstract class MembershipDomainTestBase<TStartupModule> : MembershipTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
