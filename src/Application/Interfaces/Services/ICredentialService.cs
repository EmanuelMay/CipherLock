using CipherLock.Application.DTO;

namespace CipherLock.Application.Interfaces.Services;

public interface ICredentialService
{
    public Task<ResponseCredentialDTO> AddAsync(int userId, CreateCredentialDTO dto);

    public Task<ResponseCredentialDTO> UpdateAsync(
        int userId,
        int credentialId,
        int vaultId,
        UpdateCredentialDTO dto
    );
    
    public Task<IEnumerable<ResponseCredentialDTO>> GetAllByVaultAsync(int vaultId, int userId);
}
