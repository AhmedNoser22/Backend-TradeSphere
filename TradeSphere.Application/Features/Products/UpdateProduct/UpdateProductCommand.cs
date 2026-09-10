namespace TradeSphere.Application.Features.Products.UpdateProduct;
public sealed record UpdateProductCommand(Guid Id, string Name, string Unit, string? Description)
    : IRequest<Result>, ITransactionalRequest;