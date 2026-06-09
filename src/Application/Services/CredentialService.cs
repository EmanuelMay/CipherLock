using System.Security.Cryptography;
using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Exceptions;
using CipherLock.Domain.Interfaces;

namespace CipherLock.Application.Services;

public class CredentialService(
    ICredentialRepository credentialRepository,
    IVaultRepository vaultRepository
) : ICredentialService
{
    public async Task<ResponseCredentialDTO> AddAsync(int userId, CreateCredentialDTO dto)
    {
        await GetOrThrowVaultAsync(userId, dto.VaultId);

        var iv = RandomNumberGenerator.GetBytes(16);
        var iv64 = Convert.ToBase64String(iv);

        var credential = new Credential(
            dto.Title,
            dto.Username,
            dto.VaultId,
            dto.EncryptedPassword,
            iv64
        );

        await credentialRepository.AddAsync(credential);
        await credentialRepository.SaveChangesAsync();

        return ToDTO(credential);
    }

    private ResponseCredentialDTO ToDTO(Credential credential)
    {
        return new ResponseCredentialDTO
        {
            Id = credential.Id,
            Title = credential.Title,
            Username = credential.Username,
            VaultId = credential.VaultId,
            EncryptedPassword = credential.EncryptedPassword
        };
    }

    private async Task<Vault> GetOrThrowVaultAsync(int userId, int vaultId)
    {
        var vault = await vaultRepository.GetByIdAsync(userId, vaultId)
            ?? throw new CredentialNotFoundException("credential not found");
        return vault;
    }
}
