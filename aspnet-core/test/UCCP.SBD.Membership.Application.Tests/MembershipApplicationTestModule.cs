using Volo.Abp.Modularity;

namespace UCCP.SBD.Membership;

[DependsOn(
    typeof(MembershipApplicationModule),
    typeof(MembershipDomainTestModule)
)]
public class MembershipApplicationTestModule : AbpModule
{

}
