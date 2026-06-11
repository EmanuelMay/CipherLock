using CipherLock.Application.DTO;
using CipherLock.Domain.Exceptions;
using CipherLock.Application.Interfaces.Repositories;
using CipherLock.Application.Interfaces.Services;

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
        
        if (!user.IsActive)
            throw new UserNotActiveException("account is not confirmed");

        return tokenService.Generate(user);
    }
}
