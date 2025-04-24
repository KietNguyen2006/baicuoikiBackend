namespace ToDoListAPI.Models.Auth
{
    public static class RoleConstants
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static readonly string[] AllRoles = new string[] { Admin, User };

        public static bool IsValidRole(string role)
        {
            return AllRoles.Contains(role);
        }
    }
}