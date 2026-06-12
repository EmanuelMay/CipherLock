namespace CipherLock.Infrastructure.Services;

public interface IEncryptService
{
    public string Encrypt(string plainText);

    public string Decrypt(string cipher, string iv);
}
