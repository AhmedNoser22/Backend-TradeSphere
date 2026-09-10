namespace TradeSphere.Application.Features.Suppliers.CreateSupplier;
public sealed record CreateSupplierCommand(string Name, string Country, string Email, string? Phone)
    : IRequest<Result<Guid>>, ITransactionalRequest;