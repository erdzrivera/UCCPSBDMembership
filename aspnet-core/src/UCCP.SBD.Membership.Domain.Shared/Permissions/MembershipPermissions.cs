namespace UCCP.SBD.Membership.Permissions;

public static class MembershipPermissions
{
    public const string GroupName = "Membership";

    public static class Members
    {
        public const string Default = GroupName + ".Members";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Organizations
    {
        public const string Default = GroupName + ".Organizations";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class MembershipTypes
    {
        public const string Default = GroupName + ".MembershipTypes";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
