namespace CipherLock.Infrastructure.Services;

public interface IHashService
{
    public string HashPassword(string password);

    public bool VerifyPassword(string password, string hashedPassword);
}
