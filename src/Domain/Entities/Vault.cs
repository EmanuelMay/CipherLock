namespace CipherLock.Domain.Entities;

public class Vault
{
    private Vault() { }

    public Vault(string name)
    {
        Name = name;
        CreatedAt = DateTime.Now;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }
    public IEnumerable<Credential> Credentials { get; private set; } = [];
}
