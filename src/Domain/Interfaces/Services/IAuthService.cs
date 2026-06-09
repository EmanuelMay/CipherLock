using CipherLock.Application.DTO;

namespace CipherLock.Domain.Interfaces;

public interface IAuthService
{
    public Task<string> LoginAsync(LoginDTO dto);
}
