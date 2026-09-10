namespace TradeSphere.Application.Features.Products.GetProducts;

public sealed record GetProductsQuery(
    string? SearchTerm,
    bool? IsActive,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<PaginatedList<ProductDto>>;