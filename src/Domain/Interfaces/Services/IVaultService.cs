using CipherLock.Application.Services;

namespace CipherLock.Domain.Interfaces;

public interface IVaultService
{
    public Task<ResponseVaultDTO> AddAsync(int id, CreateVaultDTO dto);

    public Task<IEnumerable<ResponseVaultDTO>> SearchByNameAsync(int userId, string name);

    public Task<ResponseVaultDTO> UpdateAsync(int userId, int vaultId, UpdateVaultDTO dto);

    public Task<ResponseVaultDetailDTO> GetByIdWithCredentialsAsync(int userId, int vaultId);
}
