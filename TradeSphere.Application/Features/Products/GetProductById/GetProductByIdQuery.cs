namespace TradeSphere.Application.Features.Products.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;