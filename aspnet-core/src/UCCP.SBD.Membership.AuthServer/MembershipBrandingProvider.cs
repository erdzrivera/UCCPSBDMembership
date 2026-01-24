using Microsoft.Extensions.Localization;
using UCCP.SBD.Membership.Localization;
using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace UCCP.SBD.Membership;

[Dependency(ReplaceServices = true)]
public class MembershipBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<MembershipResource> _localizer;

    public MembershipBrandingProvider(IStringLocalizer<MembershipResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
