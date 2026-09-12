namespace TradeSphere.Application.Features.Inventory.GetList;
public sealed class GetInventoryStocksQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetInventoryStocksQuery, PaginatedList<InventoryStockListItemDto>>
{
    public async Task<PaginatedList<InventoryStockListItemDto>> Handle(GetInventoryStocksQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.InventoryStocks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(s => s.Product.Name.Contains(request.SearchTerm) || s.Product.Sku.Contains(request.SearchTerm));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(s => s.Product.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(s => new InventoryStockListItemDto(s.ProductId, s.Product.Sku, s.Product.Name, s.QuantityOnHand))
            .ToListAsync(cancellationToken);

        return new PaginatedList<InventoryStockListItemDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}