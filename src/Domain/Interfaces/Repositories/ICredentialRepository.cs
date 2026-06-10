using CipherLock.Domain.Entities;

namespace CipherLock.Domain.Interfaces;

public interface ICredentialRepository
{
    public Task SaveChangesAsync();

    public Task AddAsync(Credential credential);

    public Task<Credential?> GetByIdAsync(int userId, int credentialId);
}
