using Volo.Abp.Modularity;

namespace UCCP.SBD.Membership;

[DependsOn(
    typeof(MembershipDomainModule),
    typeof(MembershipTestBaseModule)
)]
public class MembershipDomainTestModule : AbpModule
{

}
