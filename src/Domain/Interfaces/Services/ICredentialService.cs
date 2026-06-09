using CipherLock.Application.DTO;

namespace CipherLock.Domain.Interfaces;

public interface ICredentialService
{
    public Task<ResponseCredentialDTO> Add(int userId, CreateCredentialDTO dto);
}
