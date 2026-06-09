using CipherLock.Domain.Entities;

namespace CipherLock.Domain.Interfaces;

public interface ICredentialRepository
{
    public Task SaveChanges();

    public Task Add(Credential credential);
}
