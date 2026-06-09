using CipherLock.Application.Services;

namespace CipherLock.Domain.Interfaces;

public interface IVaultService
{
    public Task<ResponseVaultDTO> Add(int id, CreateVaultDTO dto);

    public Task<IEnumerable<ResponseVaultDTO>> SearchByName(int userId, string name);

    public Task<ResponseVaultDTO> Update(int userId, int vaultId, UpdateVaultDTO dto);

    public Task<ResponseVaultDetailDTO> GetByIdWithCredentials(int userId, int vaultId);
}
