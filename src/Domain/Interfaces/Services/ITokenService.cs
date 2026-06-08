using CipherLock.Domain.Entities;

namespace CipherLock.Domain.Interfaces;

public interface ITokenService
{
    public string Generate(User user);
}
