using UCCP.SBD.Membership.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace UCCP.SBD.Membership.Permissions;

public class MembershipPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(MembershipPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(MembershipPermissions.MyPermission1, L("Permission:MyPermission1"));

        var membersPermission = myGroup.AddPermission(MembershipPermissions.Members.Default, L("Permission:Members"));
        membersPermission.AddChild(MembershipPermissions.Members.Create, L("Permission:Create"));
        membersPermission.AddChild(MembershipPermissions.Members.Edit, L("Permission:Edit"));
        membersPermission.AddChild(MembershipPermissions.Members.Delete, L("Permission:Delete"));

        var orgPermission = myGroup.AddPermission(MembershipPermissions.Organizations.Default, L("Permission:Organizations"));
        orgPermission.AddChild(MembershipPermissions.Organizations.Create, L("Permission:Create"));
        orgPermission.AddChild(MembershipPermissions.Organizations.Edit, L("Permission:Edit"));
        orgPermission.AddChild(MembershipPermissions.Organizations.Delete, L("Permission:Delete"));

        var typesPermission = myGroup.AddPermission(MembershipPermissions.MembershipTypes.Default, L("Permission:MembershipTypes"));
        typesPermission.AddChild(MembershipPermissions.MembershipTypes.Create, L("Permission:Create"));
        typesPermission.AddChild(MembershipPermissions.MembershipTypes.Edit, L("Permission:Edit"));
        typesPermission.AddChild(MembershipPermissions.MembershipTypes.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MembershipResource>(name);
    }
}
