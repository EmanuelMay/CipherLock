using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using CipherLock.Domain.Enums;

namespace CipherLock.Domain.Entities;

public class User
{
    private User() { }

    public User(string name, string email, string passwordHash)
    {
        Validation(name, email, passwordHash);

        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = UserRoles.User;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public bool IsDeleted { get; private set; } = false;
    public UserRoles Role { get; private set; }
    public ICollection<Vault> Vaults { get; private set; } = [];

    public void ChangeRole(UserRoles role)
    {
        Role = role;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void Update(string? name, string? email)
    {
        UpdateValidation(name, email);

        if (!string.IsNullOrWhiteSpace(name))
            Name = name;
        if (!string.IsNullOrWhiteSpace(email))
            Email = email;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    private void UpdateValidation(string? name, string? email)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            if (name.Length > 150)
                throw new ArgumentException("the name cannot be longer than 150");
        }
        if (!string.IsNullOrWhiteSpace(email))
        {
            if (email.Length > 255)
                throw new ArgumentException("the email cannot be longer than 255");
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("invalid email");
        }
    }

    private static void Validation(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("name cannot be null");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("email cannot be null");
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("password cannot be null");
        if (name.Length > 150)
            throw new ArgumentException("the name cannot be longer than 150");
        if (email.Length > 255)
            throw new ArgumentException("the email cannot be longer than 255");
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("invalid email");
    }
}
