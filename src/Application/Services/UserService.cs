using System.Security.Cryptography;
using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Interfaces;

namespace CipherLock.Application.Services;

public class UserService(
    IUserRepository repository,
    IEmailService emailService
) : IUserService
{
    public async Task<ResponseUserDTO> Add(CreateUserDTO dto)
    {
        if (dto.Password.Length < 8)
            throw new Exception();
        
        var user = new User(dto.Name, dto.Email, BCrypt.Net.BCrypt.HashPassword(dto.Password));

        await repository.Add(user);
        await repository.SaveChanges();

        return ToDTO(user);
    }

    public async Task<ResponseUserDTO> GetById(int id)
    {
        var user = await GetByIdOrThrow(id);

        return ToDTO(user);
    }

    public async Task Delete(int id)
    {
        var user = await GetByIdOrThrow(id);

        user.Delete();
        await repository.SaveChanges();
    }

    public async Task<ResponseUserDTO> Update(int id, UpdateUserDTO dto)
    {
        if (await repository.EmailExists(dto.Email))
            throw new Exception();

        var user = await GetByIdOrThrow(id);

        user.Update(dto.Name, dto.Email);
        await repository.SaveChanges();

        return ToDTO(user);
    }
    
    public async Task ForgotPassword(ForgotPasswordDTO dto)
    {
        var user = await GetByEmailOrThrow(dto.Email);

        var code = RandomNumberGenerator.GetInt32(100_000, 999_999).ToString();

        var resetPassword = new UserResetPassword(code, user.Id);

        await repository.AddUserResetPassword(resetPassword);
        await repository.SaveChanges();

        await emailService.ResetPasswordEmail(user.Email, user.Name, code);
    }

    public async Task ResetPassword(ResetPasswordDTO dto)
    {
        if (dto.Password.Length < 8)
            throw new Exception();

        var user = await GetByEmailOrThrow(dto.Email);
        var resetPassword = await repository.GetUserResetPassword(user.Id, dto.Code)
            ?? throw new Exception();

        if (resetPassword.Code != dto.Code)
            throw new Exception();

        user.UpdatePassword(BCrypt.Net.BCrypt.HashPassword(dto.Password));
        await repository.SaveChanges();
    }

    private async Task<User> GetByEmailOrThrow(string email)
    {
        var user = await repository.GetByEmail(email)
            ?? throw new Exception();
        return user;
    }

    private async Task<User> GetByIdOrThrow(int id)
    {
        var user = await repository.GetById(id)
            ?? throw new Exception();
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
