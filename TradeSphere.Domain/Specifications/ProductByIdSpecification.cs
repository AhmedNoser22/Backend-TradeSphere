namespace TradeSphere.Domain.Specifications;

public sealed class ProductByIdSpecification : Specification<Product>
{
    public ProductByIdSpecification(Guid id) : base(p => p.Id == id) { }
}