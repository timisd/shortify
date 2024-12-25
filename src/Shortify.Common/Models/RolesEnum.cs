namespace Shortify.Common.Models;

public enum RolesEnum
{
    User,
    Admin
}

public static class RolesEnumExtensions
{
    public static string ToFriendlyString(this RolesEnum role)
    {
        return role switch
        {
            RolesEnum.Admin => "admin",
            RolesEnum.User => "user",
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
}