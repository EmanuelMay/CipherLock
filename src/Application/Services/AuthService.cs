using CipherLock.Domain.Entities;

namespace CipherLock.Application.Services;

public class AuthService(
    TokenService tokenService
)
{
    public async Task<string> Login(User user)
    {
        return tokenService.Generate(user);
    }
}
