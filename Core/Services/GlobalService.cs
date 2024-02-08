using DataLayer.Models.Enums;

namespace Core.Services
{
    public static class GlobalService
    {
        public static string? Name { get; set; } = "";
        public static string? Password { get; set; } = "";
        public static void Clear() => Name = Password = "";
        public static bool LoggedIn() => Name.Equals("");
        public static string[] roleNames = Enum.GetNames(typeof(Roles));
        public static int Role { get; set; }  
        public static string[] RoleNames { get; set; } = Enum.GetNames(typeof(Roles));
    }
}
