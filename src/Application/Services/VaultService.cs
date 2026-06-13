using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Exceptions;
using CipherLock.Application.Interfaces;
using CipherLock.Application.Interfaces.Repositories;
using CipherLock.Application.Interfaces.Services;
using AutoMapper;

namespace CipherLock.Application.Services;

public class VaultService(
    IVaultRepository repository,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : IVaultService
{
    public async Task<ResponseVaultDTO> AddAsync(int userId, CreateVaultDTO dto)
    {
        var vault = new Vault(dto.Name, userId);

        await repository.AddAsync(vault);
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<ResponseVaultDTO>(vault);
    }

    public async Task<IEnumerable<ResponseVaultDTO>> SearchByNameAsync(
        int userId,
        string name
    )
    {
        var vaults = await repository.SearchByNameAsync(userId, name);

        return vaults.Select(x => mapper.Map<ResponseVaultDTO>(x));
    }

    public async Task<ResponseVaultDTO> UpdateAsync(int userId, int vaultId, UpdateVaultDTO dto)
    {
        var vault = await GetByIdOrThrowAsync(userId, vaultId);

        vault.Update(dto.Name);
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<ResponseVaultDTO>(vault);
    }

    public async Task<ResponseVaultDetailDTO> GetByIdWithCredentialsAsync(int userId, int vaultId)
    {
        var vault = await GetByIdOrThrowAsync(userId, vaultId);

        return mapper.Map<ResponseVaultDetailDTO>(vault);
    }

    public async Task<IEnumerable<ResponseVaultDTO>> GetAllAsync(int userId)
    {
        var vaults = await repository.GetAllAsync(userId);

        return vaults.Select(x => mapper.Map<ResponseVaultDTO>(x));
    }

    public async Task DeleteAsync(int userId, int vaultId)
    {
        var vault = await GetByIdOrThrowAsync(userId, vaultId);

        repository.Remove(vault);
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<Vault> GetByIdOrThrowAsync(int userId, int vaultId)
    {
        var vault = await repository.GetByIdAsync(userId, vaultId)
            ?? throw new VaultNotFoundException("vault not found");
        return vault;
    }
}
