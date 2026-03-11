using TaskFlow.Domain.Entities;

namespace TaskFlow.Services.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user, IEnumerable<string> roles);
    string GenerateRefreshToken();
}
