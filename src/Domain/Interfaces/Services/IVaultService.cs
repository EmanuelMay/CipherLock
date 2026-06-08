using CipherLock.Application.Services;

namespace CipherLock.Domain.Interfaces;

public interface IVaultService
{
    public Task<ResponseVaultDTO> Add(int id, CreateVaultDTO dto);
}
