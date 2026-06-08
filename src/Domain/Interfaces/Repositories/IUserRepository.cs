using CipherLock.Domain.Entities;

namespace CipherLock.Domain.Interfaces;

public interface IUserRepository
{
    public Task SaveChanges();
    
    public Task Add(User user);
    
    public Task<User?> GetById(int id);
    
    public Task<User?> GetByEmail(string email);
    
    public Task<bool> EmailExists(string? email);

    public Task AddUserResetPassword(UserResetPassword resetPassword);

    public Task<UserResetPassword?> GetUserResetPassword(int id, string code);
}
