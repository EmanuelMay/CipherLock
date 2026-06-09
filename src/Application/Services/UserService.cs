using System.Security.Cryptography;
using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Exceptions;
using CipherLock.Domain.Interfaces;

namespace CipherLock.Application.Services;

public class UserService(
    IUserRepository repository,
    IEmailService emailService
) : IUserService
{
    public async Task<ResponseUserDTO> AddAsync(CreateUserDTO dto)
    {
        if (dto.Password.Length < 8)
            throw new InvalidPasswordLengthException("The password cannot be less than 8.");
        
        var user = new User(dto.Name, dto.Email, BCrypt.Net.BCrypt.HashPassword(dto.Password));

        await repository.AddAsync(user);
        await repository.SaveChangesAsync();

        return ToDTO(user);
    }

    public async Task<ResponseUserDTO> GetByIdAsync(int id)
    {
        var user = await GetByIdOrThrowAsync(id);

        return ToDTO(user);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await GetByIdOrThrowAsync(id);

        user.Delete();
        await repository.SaveChangesAsync();
    }

    public async Task<ResponseUserDTO> UpdateAsync(int id, UpdateUserDTO dto)
    {
        var user = await GetByIdOrThrowAsync(id);

        user.Update(dto.Name, dto.Email);
        await repository.SaveChangesAsync();

        return ToDTO(user);
    }
    
    public async Task ForgotPasswordAsync(ForgotPasswordDTO dto)
    {
        var user = await GetByEmailOrThrowAsync(dto.Email);

        var code = RandomNumberGenerator.GetInt32(100_000, 999_999).ToString();

        var resetPassword = new UserResetPassword(code, user.Id);

        await repository.AddUserResetPasswordAsync(resetPassword);
        await repository.SaveChangesAsync();

        await emailService.ResetPasswordEmailAsync(user.Email, user.Name, code);
    }

    public async Task ResetPasswordAsync(ResetPasswordDTO dto)
    {
        if (dto.Password.Length < 8)
            throw new InvalidPasswordLengthException("The password cannot be less than 8.");

        var user = await GetByEmailOrThrowAsync(dto.Email);
        var resetPassword = await repository.GetUserResetPasswordAsync(user.Id, dto.Code)
            ?? throw new InvalidCredentialsException("invalid credentials");

        if (resetPassword.Code != dto.Code)
            throw new InvalidCredentialsException("invalid credentials");

        resetPassword.Use();
        await repository.SaveChangesAsync();

        user.UpdatePassword(BCrypt.Net.BCrypt.HashPassword(dto.Password));
        await repository.SaveChangesAsync();
    }

    private async Task<User> GetByEmailOrThrowAsync(string email)
    {
        var user = await repository.GetByEmailAsync(email)
            ?? throw new UserNotFoundException("user not found");
        return user;
    }

    private async Task<User> GetByIdOrThrowAsync(int id)
    {
        var user = await repository.GetByIdAsync(id)
            ?? throw new UserNotFoundException("user not found");
        return user;
    }

    private static ResponseUserDTO ToDTO(User user)
    {
        return new ResponseUserDTO()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}
