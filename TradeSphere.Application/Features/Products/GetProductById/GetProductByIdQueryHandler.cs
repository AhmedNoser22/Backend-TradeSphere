namespace TradeSphere.Application.Features.Products.GetProductById;

public sealed class GetProductByIdQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(p => p.Id == request.Id)
            .ProjectToType<ProductDto>()
            .FirstOrDefaultAsync(cancellationToken);

        return product ?? throw new NotFoundException(nameof(Product), request.Id);
    }
}