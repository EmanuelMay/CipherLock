namespace CipherLock.Domain.Entities;

public class Credential
{
    private Credential() { }

    public Credential(string title, string username, int vaultId, string encryptedPassword, string iv)
    {
        Title = title;
        Username = username;
        VaultId = vaultId;
        EncryptedPassword = encryptedPassword;
        IV = iv;
    }

    public int Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string Username { get; private set;} = null!;
    public string EncryptedPassword { get; private set; } = null!;
    public string IV { get; private set; } = null!;
    public int VaultId { get; private set; }
    public Vault Vault { get; private set; } = null!;
}
