using CipherLock.Domain.Entities;
using CipherLock.Domain.Interfaces;
using CipherLock.Infrastructure.Context;

namespace CipherLock.Infrastructure.Repositories;

public class CredentialRepository(
    AppDbContext context
) : ICredentialRepository
{
    public async Task SaveChangesAsync()
        => await context.SaveChangesAsync();
    
    public async Task AddAsync(Credential credential)
        => await context.Credentials.AddAsync(credential);
}
