using System.Reflection;

namespace SE.Application.Common.Security;

public static class Permissions
{
    public static IReadOnlyList<string> All { get; } =
        typeof(Permissions)
            .GetNestedTypes()
            .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static))
            .Where(f => f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => (string)f.GetRawConstantValue()!)
            .ToList();

    public static class Users
    {
        public const string Create = "users.create";
        public const string Read = "users.read";
    }
}
