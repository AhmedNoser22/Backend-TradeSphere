namespace TradeSphere.Application.Features.Suppliers.ActivateSupplier;

public sealed record ActivateSupplierCommand(Guid Id) : IRequest<Result>, ITransactionalRequest;