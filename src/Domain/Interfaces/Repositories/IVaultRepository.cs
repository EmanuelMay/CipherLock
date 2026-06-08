using CipherLock.Domain.Entities;

namespace CipherLock.Domain.Interfaces;

public interface IVaultRepository
{
    public Task SaveChanges();
    
    public Task Add(Vault vault);
}
