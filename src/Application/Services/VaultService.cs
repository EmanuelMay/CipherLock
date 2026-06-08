using CipherLock.Domain.Entities;
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

        return ToDTO(vault);
    }

    private ResponseVaultDTO ToDTO(Vault vault)
    {
        return new ResponseVaultDTO()
        {
            Id = vault.Id,
            Name = vault.Name,
            CreatedAt = vault.CreatedAt,
            UserId = vault.UserId
        };
    }
}
