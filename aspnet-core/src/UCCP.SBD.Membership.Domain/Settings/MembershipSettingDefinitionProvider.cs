using Volo.Abp.Settings;

namespace UCCP.SBD.Membership.Settings;

public class MembershipSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(MembershipSettings.MySetting1));
    }
}
