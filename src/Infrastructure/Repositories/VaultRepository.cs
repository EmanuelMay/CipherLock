using CipherLock.Domain.Entities;
using CipherLock.Domain.Interfaces;
using CipherLock.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CipherLock.Infrastructure.Repositories;

public class VaultRepository(
    AppDbContext context
) : IVaultRepository
{
    public async Task SaveChangesAsync()
        => await context.SaveChangesAsync();
    
    public async Task AddAsync(Vault vault)
        => await context.Vaults.AddAsync(vault);
    
    public async Task<IEnumerable<Vault>> SearchByNameAsync(int userId, string name)
        => await context.Vaults.Where(x => x.Name.Contains(name) && x.UserId == userId).ToListAsync();

    public async Task<Vault?> GetByIdAsync(int userId, int vaultId)
        => await context.Vaults.
        Include(x => x.Credentials).
        FirstOrDefaultAsync(x => x.Id == vaultId && x.UserId == userId);

    public async Task<IEnumerable<Vault>> GetAllAsync(int userId)
        => await context.Vaults.Where(x => x.UserId == userId).ToListAsync();
    
    public void Remove(Vault vault)
        => context.Vaults.Remove(vault);
}
