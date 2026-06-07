using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;
using CipherLock.Infrastructure.Repositories;

namespace CipherLock.Application.Services;

public class UserService(
    UserRepository repository
)
{
    public async Task<UserResponseDTO> Add(CreateUserDTO dto)
    {
        if (dto.Password.Length < 8)
            throw new Exception();
        
        var user = new User(dto.Name, dto.Email, dto.Password);

        await repository.Add(user);
        await repository.SaveChanges();

        return ToDTO(user);
    }

    public async Task<UserResponseDTO> GetById(int id)
    {
        var user = await GetOrThrow(id);

        return ToDTO(user);
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAll()
    {
        var users = await repository.GetAll();

        return users.Select(x => ToDTO(x));
    }

    public async Task Delete(int id)
    {
        var user = await GetOrThrow(id);

        user.Delete();
        await repository.SaveChanges();
    }

    private async Task<User> GetOrThrow(int id)
    {
        var user = await repository.GetById(id)
            ?? throw new Exception();
        return user;
    }

    private static UserResponseDTO ToDTO(User user)
    {
        return new UserResponseDTO()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}
