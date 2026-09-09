namespace TradeSphere.Application.Features.Auth.Login;

public sealed class LoginCommandHandler(
    IRepository<User> userRepository,
    IApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(new UserByEmailSpecification(request.Email), cancellationToken);

        if (user is null || !passwordHasher.Verify(user.PasswordHash, request.Password))
            return Result<AuthResponse>.Failure("Invalid email or password.");

        if (!user.IsActive)
            return Result<AuthResponse>.Failure("This account has been deactivated.");

        if (!user.EmailConfirmed)
            return Result<AuthResponse>.Failure("Please confirm your email before logging in.");

        var (accessToken, accessTokenExpiresAtUtc) = jwtTokenService.GenerateAccessToken(user);
        var (refreshTokenValue, refreshTokenExpiresAtUtc) = jwtTokenService.GenerateRefreshToken();

        var refreshToken = user.IssueRefreshToken(refreshTokenValue, refreshTokenExpiresAtUtc);

        // Explicitly tell EF this is a brand new row — don't rely on it
        // guessing from the collection alone (see the concurrency bug this fixed).
        dbContext.RefreshTokens.Add(refreshToken);

        return Result<AuthResponse>.Success(new AuthResponse(
            user.Id, user.FullName, user.Email, user.Role, accessToken, accessTokenExpiresAtUtc, refreshTokenValue));
    }
}