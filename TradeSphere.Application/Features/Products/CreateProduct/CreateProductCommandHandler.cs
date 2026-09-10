namespace TradeSphere.Application.Features.Products.CreateProduct;

public sealed class CreateProductCommandHandler(IRepository<Product> productRepository) : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (await productRepository.AnyAsync(new ProductBySkuSpecification(request.Sku), cancellationToken))
            return Result<Guid>.Failure("A product with this SKU already exists.");

        var product = new Product(request.Sku, request.Name, request.Unit, request.Description);
        await productRepository.AddAsync(product, cancellationToken);

        return Result<Guid>.Success(product.Id);
    }
}