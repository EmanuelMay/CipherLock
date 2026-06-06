namespace CipherLock.Domain.Entities;

public class Credential
{
    private Credential() { }

    public Credential(string title, string passwordHash, int vaultId)
    {
        Title = title;
        PasswordHash = passwordHash;
        VaultId = vaultId;
    }

    public int Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string EncryptedPassword { get; private set; } = null!;
    public string IV { get; private set; } = null!;
    public int VaultId { get; private set; }
    public Vault Vault { get; private set; } = null!;
}
