namespace TradeSphere.Application.Features.Products.DeactivateProduct;

public sealed record DeactivateProductCommand(Guid Id) : IRequest<Result>, ITransactionalRequest;