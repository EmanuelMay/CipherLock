using System.Security.Cryptography;
using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Exceptions;
using CipherLock.Application.Interfaces.Repositories;
using CipherLock.Application.Interfaces.Services;
using AutoMapper;

namespace CipherLock.Application.Services;

public class UserService(
    IUserRepository repository,
    IEmailService emailService,
    IMapper mapper
) : IUserService
{
    public async Task<ResponseUserDTO> AddAsync(CreateUserDTO dto)
    {
        if (dto.Password.Length < 8)
            throw new InvalidPasswordLengthException("the password cannot be less than 8.");
        
        if (await repository.EmailExistsAsync(dto.Email))
        {
            await emailService.EmailAlreadyExistsAsync(dto.Email, dto.Name);
            return new ResponseUserDTO();
        }

        var user = new User(dto.Name, dto.Email, BCrypt.Net.BCrypt.HashPassword(dto.Password));
        await repository.AddAsync(user);
        await repository.SaveChangesAsync();

        var code = RandomNumberGenerator.GetInt32(100_000, 999_999).ToString();
        var welcome = new UserWelcomeConfirm(code, user.Id);
        await repository.AddUserWelcomeConfirmAsync(welcome);
        await repository.SaveChangesAsync();
        await emailService.WelcomeConfirmAsync(user.Email, user.Name, code);

        return mapper.Map<ResponseUserDTO>(user);
    }

    public async Task<ResponseUserDTO> GetByIdAsync(int id)
    {
        var user = await GetByIdOrThrowAsync(id);

        return mapper.Map<ResponseUserDTO>(user);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await GetByIdOrThrowAsync(id);

        repository.Delete(user);
        await repository.SaveChangesAsync();
    }

    public async Task<ResponseUserDTO> UpdateAsync(int id, UpdateUserDTO dto)
    {
        var user = await GetByIdOrThrowAsync(id);

        user.Update(dto.Name, dto.Email);
        await repository.SaveChangesAsync();

        return mapper.Map<ResponseUserDTO>(user);
    }
    
    public async Task ForgotPasswordAsync(ForgotPasswordDTO dto)
    {
        var user = await repository.GetByEmailAsync(dto.Email);

        if (user is null)
            return;

        var code = RandomNumberGenerator.GetInt32(100_000, 999_999).ToString();

        var resetPassword = new UserResetPassword(code, user.Id);

        await repository.AddUserResetPasswordAsync(resetPassword);
        await repository.SaveChangesAsync();

        await emailService.ResetPasswordAsync(user.Email, user.Name, code);
    }

    public async Task ResetPasswordAsync(ResetPasswordDTO dto)
    {
        if (dto.Password.Length < 8)
            throw new InvalidPasswordLengthException("The password cannot be less than 8.");

        var user = await repository.GetByEmailAsync(dto.Email);

        if (user is null)
            return;

        var resetPassword = await repository.GetUserResetPasswordAsync(user.Id, dto.Code)
            ?? throw new InvalidCredentialsException("invalid credentials");

        resetPassword.Use();
        await repository.SaveChangesAsync();

        user.UpdatePassword(BCrypt.Net.BCrypt.HashPassword(dto.Password));
        await repository.SaveChangesAsync();
    }

    public async Task WelcomeConfirmAsync(WelcomeConfirmDTO dto)
    {
        var user = await repository.GetByEmailAsync(dto.Email);

        if (user is null)
            return;

        var confirm = await repository.GetUserWelcomeConfirmAsync(user.Id, dto.Code)
            ?? throw new InvalidCredentialsException("invalid credentials");

        confirm.Use();
        user.Activate();
        await repository.SaveChangesAsync();
    }

    private async Task<User> GetByIdOrThrowAsync(int id)
    {
        var user = await repository.GetByIdAsync(id)
            ?? throw new UserNotFoundException("user not found");
        return user;
    }
}
