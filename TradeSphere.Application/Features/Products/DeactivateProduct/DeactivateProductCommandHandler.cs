namespace TradeSphere.Application.Features.Products.DeactivateProduct;

public sealed class DeactivateProductCommandHandler(IRepository<Product> productRepository) : IRequestHandler<DeactivateProductCommand, Result>
{
    public async Task<Result> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        product.Deactivate();
        productRepository.Update(product);

        return Result.Success();
    }
}