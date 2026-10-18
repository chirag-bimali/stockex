namespace SE.Domain.Entities;
public class Role
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; private set; }
        = new List<UserRole>();

    private Role() { } // EF Core

    public Role(string name, string description)
    {
        Name = name.Trim();
        Description = description.Trim();
    }
}
