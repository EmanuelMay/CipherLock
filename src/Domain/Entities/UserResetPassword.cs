namespace CipherLock.Domain.Entities;

public class UserResetPassword
{
    private UserResetPassword() { }

    public UserResetPassword(string code, int userId)
    {
        Code = code;
        CreatedAt = DateTime.UtcNow;
        ExpiresIn = DateTime.UtcNow.AddMinutes(30);
        IsUsed = false;
        UserId = userId;
    }

    public int Id { get; private set; }
    public string Code { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresIn { get; private set; }
    public bool IsUsed { get; private set; }
    public int UserId { get; private set; }
    public User User { get; private set; } = null!;

    public void Use()
    {
        IsUsed = true;
    }
}
