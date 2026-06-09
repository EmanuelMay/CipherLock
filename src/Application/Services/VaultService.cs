using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Exceptions;
using CipherLock.Domain.Interfaces;

namespace CipherLock.Application.Services;

public class VaultService(
    IVaultRepository repository
) : IVaultService
{
    public async Task<ResponseVaultDTO> Add(int id, CreateVaultDTO dto)
    {
        var vault = new Vault(dto.Name, id);

        await repository.Add(vault);
        await repository.SaveChanges();

        return ToResponseDTO(vault);
    }

    public async Task<IEnumerable<ResponseVaultDTO>> SearchByName(
        int userId,
        string name
    )
    {
        var vaults = await repository.SearchByName(userId, name);

        return vaults.Select(x => ToResponseDTO(x));
    }

    public async Task<ResponseVaultDTO> Update(int userId, int vaultId, UpdateVaultDTO dto)
    {
        var vault = await GetByIdOrThrow(userId, vaultId);

        vault.Update(dto.Name);
        await repository.SaveChanges();

        return ToResponseDTO(vault);
    }

    public async Task<ResponseVaultDetailDTO> GetByIdWithCredentials(int userId, int vaultId)
    {
        var vault = await GetByIdOrThrow(userId, vaultId);

        return ToResponseDetailDTO(vault);
    }

    private async Task<Vault> GetByIdOrThrow(int userId, int vaultId)
    {
        var vault = await repository.GetById(userId, vaultId)
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
