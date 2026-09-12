namespace TradeSphere.Domain.Specifications;

public sealed class InventoryStockByProductIdSpecification : Specification<InventoryStock>
{
    public InventoryStockByProductIdSpecification(Guid productId) : base(s => s.ProductId == productId) { }
}