namespace TradeSphere.Api.Filters;
public sealed class RequireRoleAttribute : AuthorizeAttribute
{
    public RequireRoleAttribute(params UserRole[] roles)
    {
        Roles = string.Join(",", roles);
    }
}