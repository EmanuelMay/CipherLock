using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Infrastructure.Repositories;

namespace CipherLock.Application.Services;

public class AuthService(
    UserRepository userRepository,
    TokenService tokenService
)
{
    public async Task<string> Login(LoginDTO dto)
    {
        var user = await userRepository.GetByEmail(dto.Email)
            ?? throw new Exception();
        
        if (BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception();

        return tokenService.Generate(user);
    }
}
