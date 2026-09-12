namespace TradeSphere.Domain.Specifications;

public sealed class OtherActiveAdminExistsSpecification : Specification<User>
{
    public OtherActiveAdminExistsSpecification(Guid excludingUserId)
        : base(u => u.Role == UserRole.SystemAdministrator && u.IsActive && u.Id != excludingUserId) { }
}