namespace TradeSphere.Application.Features.Suppliers.GetSupplierById;

public sealed record GetSupplierByIdQuery(Guid Id) : IRequest<SupplierDto>;