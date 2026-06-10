using System.ComponentModel.DataAnnotations;

namespace CipherLock.Application.DTO;

public class ResponseVaultDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int UserId { get; set; }
}

public class CreateVaultDTO
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = null!;
}

public class UpdateVaultDTO
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = null!;
}

public class ResponseVaultDetailDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int UserId { get; set; }
    public ICollection<ResponseCredentialDTO> Credentials { get; set; } = [];
}
