using CipherLock.Domain.Entities;
using CipherLock.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CipherLock.Infrastructure.Repositories;

public class UserRepository(
    AppDbContext context
)
{
    public async Task SaveChanges()
        => await context.SaveChangesAsync();
    
    public async Task Add(User user)
        => await context.Users.AddAsync(user);
    
    public async Task<User?> GetById(int id)
        => await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task<User?> GetByEmail(string email)
        => await context.Users.FirstOrDefaultAsync(x => x.Email == email);

    public async Task<IEnumerable<User>> GetAll()
        => await context.Users.AsNoTracking().ToListAsync();
}
