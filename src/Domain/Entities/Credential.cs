namespace CipherLock.Domain.Entities;

public class Credential
{
    private Credential() { }

    public Credential(
        string title,
        string username,
        int vaultId,
        string encryptedPassword,
        string iv
    )
    {
        Validation(title, username);

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

    public void Update(string title, string username)
    {
        if (title.Length > 150)
            throw new ArgumentException("the title cannot be longer than 150");
        if (username.Length > 150)
            throw new ArgumentException("the username cannot be longer than 150");

        if (!string.IsNullOrWhiteSpace(title))
            Title = title;
        if (!string.IsNullOrWhiteSpace(username))
            Username = username;
    }

    private void Validation(string title, string username)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("title cannot be null");
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("username cannot be null");
        if (title.Length > 150)
            throw new ArgumentException("the title cannot be longer than 150");
        if (username.Length > 150)
            throw new ArgumentException("the username cannot be longer than 150");
    }
}
