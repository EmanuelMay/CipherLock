using CipherLock.Domain.Entities;
using CipherLock.Application.Interfaces.Repositories;
using CipherLock.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CipherLock.Infrastructure.Repositories;

public class UserRepository(
    AppDbContext context
) : IUserRepository
{
    public async Task SaveChangesAsync()
        => await context.SaveChangesAsync();
    
    public async Task AddAsync(User user)
        => await context.Users.AddAsync(user);
    
    public async Task<User?> GetByIdAsync(int id)
        => await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task<User?> GetByEmailAsync(string email)
        => await context.Users.FirstOrDefaultAsync(x => x.Email == email);

    public async Task<bool> EmailExistsAsync(string? email)
        => await context.Users.AnyAsync(x => x.Email == email);

    public async Task AddUserResetPasswordAsync(UserResetPassword resetPassword)
        => await context.UserResetPasswords.AddAsync(resetPassword);

    public async Task<UserResetPassword?> GetUserResetPasswordAsync(int id, string code)
        => await context.UserResetPasswords.FirstOrDefaultAsync(
            x => x.Code == code &&
            x.ExpiresIn > DateTime.UtcNow &&
            x.UserId == id &&
            !x.IsUsed
        );
    
    public async Task AddUserWelcomeConfirmAsync(UserWelcomeConfirm confirm)
        => await context.UserWelcomeConfirms.AddAsync(confirm);
    
    public async Task<UserWelcomeConfirm?> GetUserWelcomeConfirmAsync(int userId, string code)
        => await context.UserWelcomeConfirms.FirstOrDefaultAsync(
            x => x.UserId == userId &&
            x.Code == code &&
            x.ExpiresIn > DateTime.UtcNow &&
            !x.IsUsed
        );
    
    public void Delete(User user)
        => context.Users.Remove(user);
}
