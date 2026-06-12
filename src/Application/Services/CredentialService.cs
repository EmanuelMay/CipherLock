using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Exceptions;
using CipherLock.Application.Interfaces.Repositories;
using CipherLock.Application.Interfaces.Services;
using AutoMapper;
using CipherLock.Infrastructure.Services;

namespace CipherLock.Application.Services;

public class CredentialService(
    ICredentialRepository credentialRepository,
    IVaultRepository vaultRepository,
    IMapper mapper,
    IEncryptService encryptService
) : ICredentialService
{
    public async Task<ResponseCredentialDTO> AddAsync(int userId, CreateCredentialDTO dto)
    {
        var vault = await GetOrThrowVaultAsync(userId, dto.VaultId);
        vault.Modify();

        var encrypted = encryptService.Encrypt(dto.EncryptedPassword);
        var parts = encrypted.Split(":");

        var credential = new Credential(
            dto.Title,
            dto.Username,
            dto.VaultId,
            parts[1],
            parts[0]
        );

        await credentialRepository.AddAsync(credential);
        await credentialRepository.SaveChangesAsync();

        return mapper.Map<ResponseCredentialDTO>(credential);
    }

    public async Task<ResponseCredentialDTO> UpdateAsync(
        int userId,
        int vaultId,
        int credentialId,
        UpdateCredentialDTO dto
    )
    {
        var credential = await GetOrThrowCredentialAsync(userId, credentialId, vaultId);

        string? encryptedPassword = null;
        string? iv = null;

        if (!string.IsNullOrWhiteSpace(dto.EncryptedPassword))
        {
            var encrypted = encryptService.Encrypt(dto.EncryptedPassword);
            var parts = encrypted.Split(":");
            iv = parts[0];
            encryptedPassword = parts[1];
        }

        credential.Update(dto.Title, dto.Username, encryptedPassword, iv);
        await credentialRepository.SaveChangesAsync();

        return mapper.Map<ResponseCredentialDTO>(credential);
    }

    public async Task<IEnumerable<ResponseCredentialDTO>> GetAllByVaultAsync(int vaultId, int userId)
    {
        var vaults = await credentialRepository.GetAllByVaultAsync(vaultId, userId);

        return vaults.Select(x =>
        {
            var dto = mapper.Map<ResponseCredentialDTO>(x);
            dto.EncryptedPassword = encryptService.Decrypt(x.EncryptedPassword, x.IV);
            return dto;
        });
    }

    private async Task<Vault> GetOrThrowVaultAsync(int userId, int vaultId)
    {
        var vault = await vaultRepository.GetByIdAsync(userId, vaultId)
            ?? throw new VaultNotFoundException("vault not found");
        return vault;
    }

    private async Task<Credential> GetOrThrowCredentialAsync(int userId, int credentialId, int vaultId)
    {
        var credential = await credentialRepository.GetByIdAsync(userId, credentialId, vaultId)
            ?? throw new CredentialNotFoundException("credential not found");
        return credential;
    }
}
