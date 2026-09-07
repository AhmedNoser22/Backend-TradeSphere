namespace TradeSphere.Application.Common.Interfaces;

public interface IJwtTokenService
{
    (string AccessToken, DateTimeOffset ExpiresAtUtc) GenerateAccessToken(User user);
    (string RefreshToken, DateTimeOffset ExpiresAtUtc) GenerateRefreshToken();
}