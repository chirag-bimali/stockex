namespace SE.Application.Common.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class RequirePermissionAttribute(string permission) : Attribute
{
    public string Permission { get; } = permission;
}
