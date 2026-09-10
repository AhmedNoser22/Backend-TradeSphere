namespace TradeSphere.Domain.Specifications;
public sealed class PurchaseOrderByIdWithLinesSpecification : Specification<PurchaseOrder>
{
    public PurchaseOrderByIdWithLinesSpecification(Guid id) : base(po => po.Id == id)
    {
        AddInclude(po => po.Lines);
    }
}