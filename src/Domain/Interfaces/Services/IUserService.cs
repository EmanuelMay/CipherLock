using CipherLock.Application.DTO;

namespace CipherLock.Domain.Interfaces;

public interface IUserService
{
    public Task<ResponseUserDTO> Add(CreateUserDTO dto);

    public Task<ResponseUserDTO> GetById(int id);

    public Task Delete(int id);

    public Task<ResponseUserDTO> Update(int id, UpdateUserDTO dto);
}
