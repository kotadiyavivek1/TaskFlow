using TaskFlow.Shared.DTOs.User;
namespace TaskFlow.Services.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
}
