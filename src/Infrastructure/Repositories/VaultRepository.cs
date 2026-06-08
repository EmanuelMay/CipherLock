using CipherLock.Application.Services;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Interfaces;
using CipherLock.Infrastructure.Context;

namespace CipherLock.Infrastructure.Repositories;

public class VaultRepository(
    AppDbContext context
) : IVaultRepository
{
    public async Task SaveChanges()
        => await context.SaveChangesAsync();
    
    public async Task Add(Vault vault)
        => await context.Vaults.AddAsync(vault);
}
