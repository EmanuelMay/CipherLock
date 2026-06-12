namespace CipherLock.Application.Services;

public interface IEncryptService
{
    public string Encrypt(string plainText);

    public string Decrypt(string cipherText);
}
