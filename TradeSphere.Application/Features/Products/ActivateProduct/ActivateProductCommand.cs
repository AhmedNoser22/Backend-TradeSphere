namespace TradeSphere.Application.Features.Products.ActivateProduct;

public sealed record ActivateProductCommand(Guid Id) : IRequest<Result>, ITransactionalRequest;