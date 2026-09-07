namespace TradeSphere.Domain.Entities;
public sealed class User : AuditableEntity
{
    public string FullName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; } = true;

    public bool EmailConfirmed { get; private set; }
    public string? EmailConfirmationCode { get; private set; }
    public DateTimeOffset? EmailConfirmationCodeExpiresAtUtc { get; private set; }

    public string? PasswordResetCode { get; private set; }
    public DateTimeOffset? PasswordResetCodeExpiresAtUtc { get; private set; }

    private readonly List<RefreshToken> _refreshTokens = [];
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User() { } // for EF Core

    public User(string fullName, string email, string passwordHash, UserRole role, string emailConfirmationCode, DateTimeOffset emailConfirmationCodeExpiresAtUtc)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        EmailConfirmationCode = emailConfirmationCode;
        EmailConfirmationCodeExpiresAtUtc = emailConfirmationCodeExpiresAtUtc;

        // Application's event handler listens for this and sends the SMTP email —
        // User has no idea an email is even involved.
        RaiseDomainEvent(new UserRegisteredEvent(Id, Email, FullName, emailConfirmationCode));
    }

    public void ConfirmEmail(string code)
    {
        if (EmailConfirmed)
            throw new BusinessRuleViolationException("Email is already confirmed.");
        if (EmailConfirmationCode != code || EmailConfirmationCodeExpiresAtUtc < DateTimeOffset.UtcNow)
            throw new BusinessRuleViolationException("Invalid or expired confirmation code.");

        EmailConfirmed = true;
        EmailConfirmationCode = null;
        EmailConfirmationCodeExpiresAtUtc = null;
    }

    public void ResendEmailConfirmationCode(string code, DateTimeOffset expiresAtUtc)
    {
        if (EmailConfirmed)
            throw new BusinessRuleViolationException("Email is already confirmed.");

        EmailConfirmationCode = code;
        EmailConfirmationCodeExpiresAtUtc = expiresAtUtc;
        RaiseDomainEvent(new EmailConfirmationCodeResentEvent(Id, Email, code));
    }

    public void RequestPasswordReset(string code, DateTimeOffset expiresAtUtc)
    {
        PasswordResetCode = code;
        PasswordResetCodeExpiresAtUtc = expiresAtUtc;
        RaiseDomainEvent(new PasswordResetRequestedEvent(Id, Email, code));
    }

    public void ResetPassword(string code, string newPasswordHash)
    {
        if (PasswordResetCode != code || PasswordResetCodeExpiresAtUtc < DateTimeOffset.UtcNow)
            throw new BusinessRuleViolationException("Invalid or expired password reset code.");

        PasswordHash = newPasswordHash;
        PasswordResetCode = null;
        PasswordResetCodeExpiresAtUtc = null;
    }

    public RefreshToken IssueRefreshToken(string token, DateTimeOffset expiresAtUtc)
    {
        var refreshToken = new RefreshToken(Id, token, expiresAtUtc);
        _refreshTokens.Add(refreshToken);
        return refreshToken;
    }

    public void Deactivate() => IsActive = false;
    public void ChangeRole(UserRole role) => Role = role;
}