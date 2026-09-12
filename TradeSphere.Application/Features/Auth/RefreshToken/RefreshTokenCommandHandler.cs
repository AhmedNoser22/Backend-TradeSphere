namespace TradeSphere.Application.Features.Auth.RefreshToken;
public sealed class RefreshTokenCommandHandler(
    IRepository<User> userRepository,
    IJwtTokenService jwtTokenService,
    IApplicationDbContext dbContext) : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(new UserByRefreshTokenSpecification(request.RefreshToken), cancellationToken);
        var existingToken = user?.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshToken);

        if (user is null || existingToken is null || !existingToken.IsActive)
            return Result<AuthResponse>.Failure("Invalid or expired refresh token.");

        var (accessToken, accessTokenExpiresAtUtc) = jwtTokenService.GenerateAccessToken(user);
        var (newRefreshTokenValue, refreshTokenExpiresAtUtc) = jwtTokenService.GenerateRefreshToken();

        existingToken.Revoke(newRefreshTokenValue);
        var newToken = user.IssueRefreshToken(newRefreshTokenValue, refreshTokenExpiresAtUtc);
        await dbContext.Set<Domain.Entities.RefreshToken>().AddAsync(newToken, cancellationToken);

        return Result<AuthResponse>.Success(new AuthResponse(
            user.Id, user.FullName, user.Email, user.Role, accessToken, accessTokenExpiresAtUtc, newRefreshTokenValue));
    }
}