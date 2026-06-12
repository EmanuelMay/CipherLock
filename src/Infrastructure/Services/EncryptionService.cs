using System.Security.Cryptography;
using System.Text;

namespace CipherLock.Infrastructure.Services;

public class EncryptionService : IEncryptService
{
    private readonly byte[] key;

    public EncryptionService(IConfiguration configuration)
    {
        var rawKey = configuration["AES:Key"]
            ?? throw new Exception();
        
        key = SHA256.HashData(Encoding.UTF8.GetBytes(rawKey));
    }

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Key = key;
        aes.GenerateIV();

        var encryptor = aes.CreateEncryptor();

        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);
        sw.Write(plainText);
        sw.Close();

        var cipherBytes = ms.ToArray();

        return Convert.ToBase64String(aes.IV) + ":" + Convert.ToBase64String(cipherBytes);
    }

    public string Decrypt(string cipher, string iv)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Key = key;
        aes.IV = Convert.FromBase64String(iv);

        var decryptor = aes.CreateDecryptor();

        using var ms = new MemoryStream(Convert.FromBase64String(cipher));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}
