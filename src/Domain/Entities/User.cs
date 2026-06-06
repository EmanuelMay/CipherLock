namespace CipherLock.Domain.Entities;

public class User
{
    private User() { }

    public User(string name, string email, string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public IEnumerable<Vault> Vaults { get; private set; } = [];
}
