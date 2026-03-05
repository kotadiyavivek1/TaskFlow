using System.Runtime.InteropServices;
using AutoMapper;
using TaskFlow.Domain.Entities;
using TaskFlow.Shared.DTOs.User;

namespace TaskFlow.Services.AutoMapper;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDto,User>()
            .ForMember(dest => dest.IsActive , opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt , opt => opt.MapFrom(src => DateTime.UtcNow));

            // .ForMember(dest => dest.UserRoles )
    }
}
