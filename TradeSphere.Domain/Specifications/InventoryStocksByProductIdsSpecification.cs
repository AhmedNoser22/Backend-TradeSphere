namespace TradeSphere.Domain.Specifications;
public sealed class InventoryStocksByProductIdsSpecification : Specification<InventoryStock>
{
    public InventoryStocksByProductIdsSpecification(IEnumerable<Guid> productIds)
        : base(s => productIds.Contains(s.ProductId)) { }
}