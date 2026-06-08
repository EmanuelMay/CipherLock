using CipherLock.Domain.Entities;
using CipherLock.Domain.Interfaces;
using CipherLock.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CipherLock.Infrastructure.Repositories;

public class UserRepository(
    AppDbContext context
) : IUserRepository
{
    public async Task SaveChanges()
        => await context.SaveChangesAsync();
    
    public async Task Add(User user)
        => await context.Users.AddAsync(user);
    
    public async Task<User?> GetById(int id)
        => await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task<User?> GetByEmail(string email)
        => await context.Users.FirstOrDefaultAsync(x => x.Email == email);

    public async Task<bool> EmailExists(string? email)
        => await context.Users.AnyAsync(x => x.Email == email);

    public async Task AddUserResetPassword(UserResetPassword resetPassword)
        => await context.UserResetPasswords.AddAsync(resetPassword);

    public async Task<UserResetPassword?> GetUserResetPassword(int id, string code)
        => await context.UserResetPasswords.FirstOrDefaultAsync(x => x.Code == code && x.CreatedAt < x.ExpiresIn && x.UserId == id && !x.IsUsed);
}
