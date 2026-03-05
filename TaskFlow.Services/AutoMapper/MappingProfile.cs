using AutoMapper;
using TaskFlow.Domain.Entities;
using TaskFlow.Shared.DTOs;

namespace TaskFlow.Services.AutoMapper;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDto,User>();
    }
}
