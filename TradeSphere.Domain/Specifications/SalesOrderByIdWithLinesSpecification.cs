namespace TradeSphere.Domain.Specifications;

public sealed class SalesOrderByIdWithLinesSpecification : Specification<SalesOrder>
{
    public SalesOrderByIdWithLinesSpecification(Guid id) : base(so => so.Id == id)
    {
        AddInclude(so => so.Lines);
    }
}