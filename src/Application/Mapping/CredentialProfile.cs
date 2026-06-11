using AutoMapper;
using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;

namespace CipherLock.Application.Mapping;

public class CredentialProfile : Profile
{
    public CredentialProfile()
    {
        CreateMap<Credential, ResponseCredentialDTO>();
    }
}
