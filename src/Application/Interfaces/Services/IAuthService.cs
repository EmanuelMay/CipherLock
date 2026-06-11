using CipherLock.Application.DTO;

namespace CipherLock.Application.Interfaces.Services;

public interface IAuthService
{
    public Task<string> LoginAsync(LoginDTO dto);
}
