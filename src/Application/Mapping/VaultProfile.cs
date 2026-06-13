using AutoMapper;
using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;

namespace CipherLock.Application.Mapping;

public class VaultProfile : Profile
{
    public VaultProfile()
    {
        CreateMap<Vault, ResponseVaultDTO>();
        CreateMap<Vault, ResponseVaultDetailDTO>();
    }
}
