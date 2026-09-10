namespace TradeSphere.Application.Features.Products.ActivateProduct;

public sealed class ActivateProductCommandHandler(IRepository<Product> productRepository) : IRequestHandler<ActivateProductCommand, Result>
{
    public async Task<Result> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        product.Activate();
        productRepository.Update(product);

        return Result.Success();
    }
}