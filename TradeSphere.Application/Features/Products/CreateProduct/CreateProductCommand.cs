namespace TradeSphere.Application.Features.Products.CreateProduct;

public sealed record CreateProductCommand(string Sku, string Name, string Unit, string? Description)
    : IRequest<Result<Guid>>, ITransactionalRequest;