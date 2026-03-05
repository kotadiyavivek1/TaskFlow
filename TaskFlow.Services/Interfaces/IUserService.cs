using TaskFlow.Shared.DTOs;
namespace TaskFlow.Services.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
}
