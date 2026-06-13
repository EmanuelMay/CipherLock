using System.Security.Cryptography;
using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Exceptions;
using CipherLock.Application.Interfaces;
using CipherLock.Application.Interfaces.Repositories;
using CipherLock.Application.Interfaces.Services;
using AutoMapper;
using CipherLock.Infrastructure.Services;

namespace CipherLock.Application.Services;

public class UserService(
    IUserRepository repository,
    IEmailService emailService,
    IMapper mapper,
    IHashService hashService,
    IUnitOfWork unitOfWork
) : IUserService
{
    public async Task AddAsync(CreateUserDTO dto)
    {
        var existingUser = await repository.GetByEmailAsync(dto.Email);

        if (existingUser is not null)
        {
            if (!existingUser!.IsActive)
            {
                await SendWelcomeCodeAsync(existingUser);
                return;
            }

            await emailService.EmailAlreadyExistsAsync(dto.Email, dto.Name);
            return;
        }

        var newUser = new User(dto.Name, dto.Email, hashService.HashPassword(dto.Password));
        await repository.AddAsync(newUser);
        await unitOfWork.SaveChangesAsync();

        await SendWelcomeCodeAsync(newUser);
    }

    public async Task<ResponseUserDTO> GetByIdAsync(int id)
    {
        var user = await GetByIdOrThrowAsync(id);

        return mapper.Map<ResponseUserDTO>(user);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await GetByIdOrThrowAsync(id);

        repository.Remove(user);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<ResponseUserDTO> UpdateAsync(int id, UpdateUserDTO dto)
    {
        var user = await GetByIdOrThrowAsync(id);

        user.Update(dto.Name, dto.Email);
        await unitOfWork.SaveChangesAsync();

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
        await unitOfWork.SaveChangesAsync();

        await emailService.ResetPasswordAsync(user.Email, user.Name, code);
    }

    public async Task ResetPasswordAsync(ResetPasswordDTO dto)
    {
        var user = await repository.GetByEmailAsync(dto.Email);

        if (user is null)
            return;

        var resetPassword = await repository.GetUserResetPasswordAsync(user.Id, dto.Code)
            ?? throw new InvalidCredentialsException("invalid credentials");

        resetPassword.Use();

        user.UpdatePassword(hashService.HashPassword(dto.Password));
        await unitOfWork.SaveChangesAsync();
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
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<User> GetByIdOrThrowAsync(int id)
    {
        var user = await repository.GetByIdAsync(id)
            ?? throw new UserNotFoundException("user not found");
        return user;
    }

    private async Task SendWelcomeCodeAsync(User user)
    {
        var code = RandomNumberGenerator.GetInt32(100_000, 999_999).ToString();
        var welcome = new UserWelcomeConfirm(code, user.Id);
        await repository.AddUserWelcomeConfirmAsync(welcome);
        await unitOfWork.SaveChangesAsync();
        await emailService.WelcomeConfirmAsync(user.Email, user.Name, code);
    }
}
