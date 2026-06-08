namespace CipherLock.Domain.Entities;

public class Vault
{
    private Vault() { }

    public Vault(string name, int userId)
    {
        Name = name;
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; } = null;
    public int UserId { get; private set; }
    public User User { get; private set; } = null!;
    public ICollection<Credential> Credentials { get; private set; } = [];
}
