using CipherLock.Application.DTO;

namespace CipherLock.Application.Interfaces.Services;

public interface IUserService
{
    public Task<ResponseUserDTO> AddAsync(CreateUserDTO dto);

    public Task<ResponseUserDTO> GetByIdAsync(int id);

    public Task DeleteAsync(int id);

    public Task<ResponseUserDTO> UpdateAsync(int id, UpdateUserDTO dto);

    public Task ForgotPasswordAsync(ForgotPasswordDTO dto);

    public Task ResetPasswordAsync(ResetPasswordDTO dto);
}
