namespace TradeSphere.Domain.Specifications;
public sealed class LowStockProductsSpecification : Specification<InventoryStock>
{
    public LowStockProductsSpecification(int threshold)
        : base(stock => stock.QuantityOnHand <= threshold)
    {
        AddInclude(stock => stock.Product);
    }
}