using CipherLock.Application.DTO;

namespace CipherLock.Domain.Interfaces;

public interface ICredentialService
{
    public Task<ResponseCredentialDTO> AddAsync(int userId, CreateCredentialDTO dto);
}
