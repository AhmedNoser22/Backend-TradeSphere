namespace TradeSphere.Domain.Specifications;

public abstract class Specification<T>(Expression<Func<T, bool>> criteria) : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; } = criteria;
    public List<Expression<Func<T, object>>> Includes { get; } = [];

    // Lets a derived specification say "and also load this related entity"
    // without Domain knowing anything about EF Core's .Include() itself.
    protected void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);
}