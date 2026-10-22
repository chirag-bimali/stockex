namespace SE.Domain.Entities;

public class Permission
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public ICollection<RolePermission> RolePermissions { get; private set; } =
        new List<RolePermission>();

    private Permission() { } // EF Core

    public Permission(string name) => Name = name;
}
