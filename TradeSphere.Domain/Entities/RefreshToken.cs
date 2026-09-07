namespace TradeSphere.Domain.Entities;
public sealed class RefreshToken : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = default!;
    public DateTimeOffset ExpiresAtUtc { get; private set; }
    public DateTimeOffset? RevokedAtUtc { get; private set; }
    public string? ReplacedByToken { get; private set; }

    public bool IsActive => RevokedAtUtc is null && DateTimeOffset.UtcNow < ExpiresAtUtc;

    private RefreshToken() { }

    internal RefreshToken(Guid userId, string token, DateTimeOffset expiresAtUtc)
    {
        UserId = userId;
        Token = token;
        ExpiresAtUtc = expiresAtUtc;
    }

    public void Revoke(string? replacedByToken = null)
    {
        RevokedAtUtc = DateTimeOffset.UtcNow;
        ReplacedByToken = replacedByToken;
    }
}