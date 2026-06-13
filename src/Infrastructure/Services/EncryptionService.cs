using System.Security.Cryptography;
using System.Text;
using CipherLock.Application.Services;

namespace CipherLock.Infrastructure.Services;

public class EncryptionService : IEncryptService
{
    private readonly byte[] key;

    public EncryptionService(IConfiguration configuration)
    {
        var rawKey = configuration["AES:Key"]
            ?? throw new Exception();
        
        var keyMaterial = SHA256.HashData(Encoding.UTF8.GetBytes(rawKey));

        key = HKDF.DeriveKey(
            hashAlgorithmName: HashAlgorithmName.SHA256,
            ikm: keyMaterial,          // input key material
            outputLength: 32,
            salt: null,                // opcional: pode passar um salt fixo por contexto
            info: Encoding.UTF8.GetBytes("cipherlock-credential-encryption")
        );
    }

    public string Encrypt(string plainText)
    {
        byte[] nonce = RandomNumberGenerator.GetBytes(12);

        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = new byte[plaintextBytes.Length];
        byte[] tag = new byte[16];

        using var aes = new AesGcm(key, 16);

        aes.Encrypt(
            nonce,
            plaintextBytes,
            cipherBytes,
            tag
        );

        return $"{Convert.ToBase64String(nonce)}:" +
            $"{Convert.ToBase64String(tag)}:" +
            $"{Convert.ToBase64String(cipherBytes)}";
    }

    public string Decrypt(string cipherText)
    {
        var parts = cipherText.Split(':');

        if (parts.Length != 3)
            throw new ArgumentException("encrypted text is invalid");
        
        byte[] nonce = Convert.FromBase64String(parts[0]);
        byte[] tag = Convert.FromBase64String(parts[1]);
        byte[] cipherBytes = Convert.FromBase64String(parts[2]);

        byte[] plainttextBytes = new byte[cipherBytes.Length];

        using var aes = new AesGcm(key, 16);

        aes.Decrypt(
            nonce,
            cipherBytes,
            tag,
            plainttextBytes
        );

        return Encoding.UTF8.GetString(plainttextBytes);
    }
}
