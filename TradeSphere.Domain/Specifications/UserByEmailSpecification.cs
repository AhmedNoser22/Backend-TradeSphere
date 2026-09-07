namespace TradeSphere.Domain.Specifications;

public sealed class UserByEmailSpecification : Specification<User>
{
    public UserByEmailSpecification(string email) : base(u => u.Email == email)
    {
        AddInclude(u => u.RefreshTokens);
    }
}