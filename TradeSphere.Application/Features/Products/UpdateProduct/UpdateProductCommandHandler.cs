namespace TradeSphere.Application.Features.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler(IRepository<Product> productRepository) : IRequestHandler<UpdateProductCommand, Result>
{
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        product.UpdateDetails(request.Name, request.Unit, request.Description);
        productRepository.Update(product);

        return Result.Success();
    }
}