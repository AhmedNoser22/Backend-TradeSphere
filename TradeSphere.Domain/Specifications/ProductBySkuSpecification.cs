namespace TradeSphere.Domain.Specifications;
public sealed class ProductBySkuSpecification : Specification<Product>
{
    public ProductBySkuSpecification(string sku) : base(p => p.Sku == sku) { }
}