using CipherLock.Application.DTO;
using CipherLock.Domain.Exceptions;
using CipherLock.Application.Interfaces.Repositories;
using CipherLock.Application.Interfaces.Services;
using CipherLock.Infrastructure.Services;

namespace CipherLock.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashService hashService
) : IAuthService
{
    public async Task<string> LoginAsync(LoginDTO dto)
    {
        var user = await userRepository.GetByEmailAsync(dto.Email)
            ?? throw new InvalidCredentialsException("invalid credentials");
        
        if (!hashService.VerifyPassword(dto.Password, user.PasswordHash))
            throw new InvalidCredentialsException("invalid credentials");
        
        if (!user.IsActive)
            throw new UserNotActiveException("account is not confirmed");

        return tokenService.Generate(user);
    }
}
