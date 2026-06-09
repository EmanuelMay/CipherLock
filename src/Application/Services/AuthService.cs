using CipherLock.Application.DTO;
using CipherLock.Domain.Exceptions;
using CipherLock.Domain.Interfaces;

namespace CipherLock.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    ITokenService tokenService
) : IAuthService
{
    public async Task<string> LoginAsync(LoginDTO dto)
    {
        var user = await userRepository.GetByEmailAsync(dto.Email)
            ?? throw new InvalidCredentialsException("invalid credentials");
        
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new InvalidCredentialsException("invalid credentials");

        return tokenService.Generate(user);
    }
}
