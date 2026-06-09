using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Exceptions;
using CipherLock.Domain.Interfaces;

namespace CipherLock.Application.Services;

public class VaultService(
    IVaultRepository repository
) : IVaultService
{
    public async Task<ResponseVaultDTO> AddAsync(int id, CreateVaultDTO dto)
    {
        var vault = new Vault(dto.Name, id);

        await repository.AddAsync(vault);
        await repository.SaveChangesAsync();

        return ToResponseDTO(vault);
    }

    public async Task<IEnumerable<ResponseVaultDTO>> SearchByNameAsync(
        int userId,
        string name
    )
    {
        var vaults = await repository.SearchByNameAsync(userId, name);

        return vaults.Select(x => ToResponseDTO(x));
    }

    public async Task<ResponseVaultDTO> UpdateAsync(int userId, int vaultId, UpdateVaultDTO dto)
    {
        var vault = await GetByIdOrThrowAsync(userId, vaultId);

        vault.Update(dto.Name);
        await repository.SaveChangesAsync();

        return ToResponseDTO(vault);
    }

    public async Task<ResponseVaultDetailDTO> GetByIdWithCredentialsAsync(int userId, int vaultId)
    {
        var vault = await GetByIdOrThrowAsync(userId, vaultId);

        return ToResponseDetailDTO(vault);
    }

    private async Task<Vault> GetByIdOrThrowAsync(int userId, int vaultId)
    {
        var vault = await repository.GetByIdAsync(userId, vaultId)
            ?? throw new VaultNotFoundException("vault not found");
        return vault;
    }

    private ResponseVaultDetailDTO ToResponseDetailDTO(Vault vault)
    {
        return new ResponseVaultDetailDTO
        {
            Id = vault.Id,
            Name = vault.Name,
            CreatedAt = vault.CreatedAt,
            ModifiedAt = vault.ModifiedAt,
            UserId = vault.UserId,
            Credentials = vault.Credentials.Select(x => new ResponseCredentialDTO
            {
                Id = x.Id,
                Title = x.Title,
                Username = x.Username,
                EncryptedPassword = x.EncryptedPassword
            }).ToList()
        };
    }

    private ResponseVaultDTO ToResponseDTO(Vault vault)
    {
        return new ResponseVaultDTO()
        {
            Id = vault.Id,
            Name = vault.Name,
            CreatedAt = vault.CreatedAt,
            ModifiedAt = vault.ModifiedAt,
            UserId = vault.UserId
        };
    }
}
