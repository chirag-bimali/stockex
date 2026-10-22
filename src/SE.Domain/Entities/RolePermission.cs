public class RolePermission
{
    public int RoleId { get; private set; }
    public Guid PermissionId { get; private set; }

    private RolePermission() { } // EF Core

    public RolePermission(int roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
}
