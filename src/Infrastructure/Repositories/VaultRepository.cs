using CipherLock.Domain.Entities;
using CipherLock.Domain.Interfaces;
using CipherLock.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CipherLock.Infrastructure.Repositories;

public class VaultRepository(
    AppDbContext context
) : IVaultRepository
{
    public async Task SaveChanges()
        => await context.SaveChangesAsync();
    
    public async Task Add(Vault vault)
        => await context.Vaults.AddAsync(vault);
    
    public async Task<IEnumerable<Vault>> SearchByName(int userId, string name)
        => await context.Vaults.Where(x => x.Name.Contains(name) && x.UserId == userId).ToListAsync();

    public async Task<Vault?> GetById(int userId, int vaultId)
        => await context.Vaults.
        Include(x => x.Credentials).
        FirstOrDefaultAsync(x => x.Id == vaultId && x.UserId == userId);
}
