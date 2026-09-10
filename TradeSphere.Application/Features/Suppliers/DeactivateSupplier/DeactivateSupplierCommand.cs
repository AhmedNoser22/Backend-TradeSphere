namespace TradeSphere.Application.Features.Suppliers.DeactivateSupplier;

public sealed record DeactivateSupplierCommand(Guid Id) : IRequest<Result>, ITransactionalRequest;