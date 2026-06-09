namespace CipherLock.Application.DTO;

public class ResponseCredentialDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string EncryptedPassword { get; set; } = null!;
    public int VaultId { get; set; }
}

public class CreateCredentialDTO
{
    public string Title { get; set; } = null!;
    public string Username { get; set; } = null!;
    public int VaultId { get; set; }
    public string EncryptedPassword { get; set; } = null!;
}
