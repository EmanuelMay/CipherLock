using CipherLock.Domain.Entities;

namespace CipherLock.Application.Interfaces.Repositories;

public interface ICredentialRepository
{
    public Task AddAsync(Credential credential);

    public Task<Credential?> GetByIdAsync(int userId, int vaultId, int credentialId);

    public Task<IEnumerable<Credential>> GetAllByVaultAsync(int vaultId, int userId);
}
