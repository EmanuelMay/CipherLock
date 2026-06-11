using System.Security.Cryptography;
using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Exceptions;
using CipherLock.Application.Interfaces.Repositories;
using CipherLock.Application.Interfaces.Services;
using AutoMapper;

namespace CipherLock.Application.Services;

public class CredentialService(
    ICredentialRepository credentialRepository,
    IVaultRepository vaultRepository,
    IMapper mapper
) : ICredentialService
{
    public async Task<ResponseCredentialDTO> AddAsync(int userId, CreateCredentialDTO dto)
    {
        var vault = await GetOrThrowVaultAsync(userId, dto.VaultId);
        vault.Modify();

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

        credential.Update(dto.Title, dto.Username);
        await credentialRepository.SaveChangesAsync();

        return mapper.Map<ResponseCredentialDTO>(credential);
    }

    public async Task<IEnumerable<ResponseCredentialDTO>> GetAllByVaultAsync(int vaultId, int userId)
    {
        var vaults = await credentialRepository.GetAllByVaultAsync(vaultId, userId);

        return vaults.Select(x => mapper.Map<ResponseCredentialDTO>(x));
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
