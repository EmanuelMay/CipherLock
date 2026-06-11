using CipherLock.Domain.Entities;

namespace CipherLock.Application.Interfaces.Repositories;

public interface IVaultRepository
{
    public Task SaveChangesAsync();
    
    public Task AddAsync(Vault vault);

    public Task<IEnumerable<Vault>> SearchByNameAsync(int id, string name);
    
    public Task<Vault?> GetByIdAsync(int userId, int vaultId);

    public Task<IEnumerable<Vault>> GetAllAsync(int userId);

    public void Remove(Vault vault);
}
