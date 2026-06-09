using CipherLock.Domain.Entities;

namespace CipherLock.Domain.Interfaces;

public interface IVaultRepository
{
    public Task SaveChangesAsync();
    
    public Task AddAsync(Vault vault);

    public Task<IEnumerable<Vault>> SearchByNameAsync(int id, string name);
    
    public Task<Vault?> GetByIdAsync(int userId, int vaultId);
}
