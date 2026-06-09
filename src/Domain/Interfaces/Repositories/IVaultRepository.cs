using CipherLock.Domain.Entities;

namespace CipherLock.Domain.Interfaces;

public interface IVaultRepository
{
    public Task SaveChanges();
    
    public Task Add(Vault vault);

    public Task<IEnumerable<Vault>> SearchByName(int id, string name);
    
    public Task<Vault?> GetById(int userId, int vaultId);
}
