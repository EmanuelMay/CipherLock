namespace CipherLock.Domain.Entities;

public class Credential
{
    private Credential() { }

    public Credential(
        string title,
        string username,
        int vaultId,
        string encryptedPassword
    )
    {
        Validation(title, username);

        Title = title;
        Username = username;
        VaultId = vaultId;
        EncryptedPassword = encryptedPassword;
    }

    public int Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string Username { get; private set;} = null!;
    public string EncryptedPassword { get; private set; } = null!;
    public int VaultId { get; private set; }
    public Vault Vault { get; private set; } = null!;

    public void Update(string? title, string? username, string? encryptedPassword)
    {
        UpdateValidation(title, username);

        if (!string.IsNullOrWhiteSpace(title))
            Title = title;
        if (!string.IsNullOrWhiteSpace(username))
            Username = username;
        if (!string.IsNullOrWhiteSpace(encryptedPassword))
            EncryptedPassword = encryptedPassword;
    }

    private void UpdateValidation(string? title, string? username)
    {
        if (!string.IsNullOrWhiteSpace(title))
            if (title.Length > 150)
                throw new ArgumentException("the name cannot be longer than 150");
        if (!string.IsNullOrWhiteSpace(username))
            if (username.Length > 150)
                throw new ArgumentException("the username cannot be longer than 150");
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
