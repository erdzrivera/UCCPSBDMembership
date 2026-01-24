using System;
using System.Collections.Generic;
using System.Text;
using UCCP.SBD.Membership.Localization;
using Volo.Abp.Application.Services;

namespace UCCP.SBD.Membership;

/* Inherit your application services from this class.
 */
public abstract class MembershipAppService : ApplicationService
{
    protected MembershipAppService()
    {
        LocalizationResource = typeof(MembershipResource);
    }
}
