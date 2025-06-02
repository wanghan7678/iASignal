using AutoMapper;
using iASignalApi.Models;
using iASignalApi.Models.Dtos;

namespace iASignalApi;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponse>();
    }
}