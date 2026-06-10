using CipherLock.Domain.Entities;
using CipherLock.Domain.Interfaces;
using CipherLock.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CipherLock.Infrastructure.Repositories;

public class CredentialRepository(
    AppDbContext context
) : ICredentialRepository
{
    public async Task SaveChangesAsync()
        => await context.SaveChangesAsync();
    
    public async Task AddAsync(Credential credential)
        => await context.Credentials.AddAsync(credential);
    
    public async Task<Credential?> GetByIdAsync(int credentialId, int vaultId)
        => await context.Credentials.FirstOrDefaultAsync(x => x.Id == credentialId && x.VaultId == vaultId);
    
    public async Task<IEnumerable<Credential>> GetAllByVaultAsync(int vaultId)
        => await context.Credentials.Where(x => x.VaultId == vaultId).ToListAsync();
}
