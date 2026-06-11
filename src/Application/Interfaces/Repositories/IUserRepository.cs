using CipherLock.Domain.Entities;

namespace CipherLock.Application.Interfaces.Repositories;

public interface IUserRepository
{
    public Task SaveChangesAsync();
    
    public Task AddAsync(User user);
    
    public Task<User?> GetByIdAsync(int id);
    
    public Task<User?> GetByEmailAsync(string email);
    
    public Task<bool> EmailExistsAsync(string? email);

    public Task AddUserResetPasswordAsync(UserResetPassword resetPassword);

    public Task<UserResetPassword?> GetUserResetPasswordAsync(int id, string code);
}
