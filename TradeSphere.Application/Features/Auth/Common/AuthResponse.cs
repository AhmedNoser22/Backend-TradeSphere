namespace TradeSphere.Application.Features.Auth.Common;
public sealed record AuthResponse(
    Guid UserId,
    string FullName,
    string Email,
    UserRole Role,
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAtUtc,
    string RefreshToken);