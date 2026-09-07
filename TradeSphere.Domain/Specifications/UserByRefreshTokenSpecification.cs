namespace TradeSphere.Domain.Specifications;
public sealed class UserByRefreshTokenSpecification : Specification<User>
{
    public UserByRefreshTokenSpecification(string refreshToken)
        : base(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken))
    {
        AddInclude(u => u.RefreshTokens);
    }
}