using UCCP.SBD.Membership.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace UCCP.SBD.Membership.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class MembershipController : AbpControllerBase
{
    protected MembershipController()
    {
        LocalizationResource = typeof(MembershipResource);
    }
}
