using AutoMapper;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Repositories.Interfaces;
using TaskFlow.Services.Interfaces;
using TaskFlow.Shared.DTOs.User;

namespace TaskFlow.Services.Implementation;

public class UserService(IGenericRepository<User> UserRepository,IMapper mapper) : IUserService
{
    public async Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        mapper.Map<User>(registerUserDto);
        await UserRepository.AddAsync(mapper.Map<User>(registerUserDto));
        return true;
    }
}
