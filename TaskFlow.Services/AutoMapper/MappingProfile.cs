using AutoMapper;
using TaskFlow.Domain.Entities;
using TaskFlow.Shared.DTOs.User;

namespace TaskFlow.Services.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // RegisterUserDto → User
        // Password field is NOT mapped automatically; AuthService hashes it manually.
        CreateMap<RegisterUserDto, User>()
            .ForMember(dest => dest.Email,       opt => opt.MapFrom(src => src.Email.ToLowerInvariant()))
            .ForMember(dest => dest.IsActive,    opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedAt,   opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.UserRoles,   opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
    }
}
