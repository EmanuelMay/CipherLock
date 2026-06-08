using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Interfaces;
using CipherLock.Infrastructure.Repositories;

namespace CipherLock.Application.Services;

public class UserService(
    IUserRepository repository
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
        var user = await GetOrThrow(id);

        return ToDTO(user);
    }

    public async Task Delete(int id)
    {
        var user = await GetOrThrow(id);

        user.Delete();
        await repository.SaveChanges();
    }

    public async Task<ResponseUserDTO> Update(int id, UpdateUserDTO dto)
    {
        if (await repository.EmailExists(dto.Email))
            throw new Exception();

        var user = await GetOrThrow(id);

        user.Update(dto.Name, dto.Email);

        return ToDTO(user);
    }

    private async Task<User> GetOrThrow(int id)
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
