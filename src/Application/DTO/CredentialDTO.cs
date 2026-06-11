using System.ComponentModel.DataAnnotations;

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
    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public int VaultId { get; set; }

    [Required]
    public string EncryptedPassword { get; set; } = null!;
}

public class UpdateCredentialDTO
{
    [Required]
    public string? Title { get; set; } = null!;

    [Required]
    public string? Username { get; set; } = null!;
}
