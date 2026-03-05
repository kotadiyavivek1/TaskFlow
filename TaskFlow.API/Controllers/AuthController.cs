using Microsoft.AspNetCore.Mvc;
using TaskFlow.Services.Interfaces;
using TaskFlow.Shared.DTOs;
namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        bool user = await userService.RegisterUserAsync(dto);

        return Ok(new
        {
            message = "User registered successfully",
            data = user
        });
    }
}
