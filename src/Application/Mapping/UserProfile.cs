using AutoMapper;
using CipherLock.Application.DTO;
using CipherLock.Domain.Entities;

namespace CipherLock.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, ResponseUserDTO>();
    }
}
