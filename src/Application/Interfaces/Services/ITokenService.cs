using CipherLock.Domain.Entities;

namespace CipherLock.Application.Interfaces.Services;

public interface ITokenService
{
    public string Generate(User user);
}
