namespace TradeSphere.Application.Features.Suppliers.GetSuppliers;

public sealed record GetSuppliersQuery(
    string? SearchTerm,
    bool? IsActive,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<PaginatedList<SupplierDto>>;