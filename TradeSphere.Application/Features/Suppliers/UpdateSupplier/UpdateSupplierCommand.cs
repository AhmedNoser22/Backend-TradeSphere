namespace TradeSphere.Application.Features.Suppliers.UpdateSupplier;

public sealed record UpdateSupplierCommand(Guid Id, string Name, string Country, string Email, string? Phone)
    : IRequest<Result>, ITransactionalRequest;