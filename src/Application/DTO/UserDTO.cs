using System.ComponentModel.DataAnnotations;
using CipherLock.Domain.Enums;

namespace CipherLock.Application.DTO;

public class ResponseUserDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
}

public class CreateUserDTO
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(8)]
    public string Password { get; set; } = null!;
}

public class LoginDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(8)]
    public string Password { get; set; } = null!;
}

public class UpdateUserDTO
{
    public string? Name { get; set; }
    public string? Email { get; set; }
}

public class ForgotPasswordDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}

public class ResetPasswordDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required] 
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(8)]
    public string Password { get; set; } = null!;
}

public class WelcomeConfirmDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Code { get; set; } = null!;
}
