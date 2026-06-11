using CipherLock.Application.DTO;

namespace CipherLock.Application.Interfaces.Services;

public interface IVaultService
{
    public Task<ResponseVaultDTO> AddAsync(int userId, CreateVaultDTO dto);

    public Task<IEnumerable<ResponseVaultDTO>> SearchByNameAsync(int userId, string name);

    public Task<ResponseVaultDTO> UpdateAsync(int userId, int vaultId, UpdateVaultDTO dto);

    public Task<ResponseVaultDetailDTO> GetByIdWithCredentialsAsync(int userId, int vaultId);

    public Task<IEnumerable<ResponseVaultDTO>> GetAllAsync(int userId);

    public Task DeleteAsync(int userId, int vaultId);
}
