namespace TradeSphere.Application.Features.Auth.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    IRepository<User> userRepository,
    IApplicationDbContext dbContext,
    IJwtTokenService jwtTokenService) : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = (await userRepository.ListAsync(new UserByRefreshTokenSpecification(request.RefreshToken), cancellationToken)).FirstOrDefault();
        var existingToken = user?.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshToken);

        if (user is null || existingToken is null || !existingToken.IsActive)
            return Result<AuthResponse>.Failure("Invalid or expired refresh token.");

        var (accessToken, accessTokenExpiresAtUtc) = jwtTokenService.GenerateAccessToken(user);
        var (newRefreshTokenValue, refreshTokenExpiresAtUtc) = jwtTokenService.GenerateRefreshToken();

        // existingToken is already tracked (it came from the query), so this
        // Revoke() is picked up by EF automatically — no explicit Add needed here.
        existingToken.Revoke(newRefreshTokenValue);

        var newRefreshToken = user.IssueRefreshToken(newRefreshTokenValue, refreshTokenExpiresAtUtc);
        dbContext.RefreshTokens.Add(newRefreshToken);

        return Result<AuthResponse>.Success(new AuthResponse(
            user.Id, user.FullName, user.Email, user.Role, accessToken, accessTokenExpiresAtUtc, newRefreshTokenValue));
    }
}