namespace TradeSphere.Application.Features.Inventory.GetLowStock;
public sealed class GetLowStockProductsQueryHandler(IRepository<InventoryStock> inventoryRepository)
    : IRequestHandler<GetLowStockProductsQuery, List<InventoryStockListItemDto>>
{
    public async Task<List<InventoryStockListItemDto>> Handle(GetLowStockProductsQuery request, CancellationToken cancellationToken)
    {
        var stocks = await inventoryRepository.ListAsync(new LowStockProductsSpecification(request.Threshold), cancellationToken);

        return stocks
            .Select(s => new InventoryStockListItemDto(s.ProductId, s.Product.Sku, s.Product.Name, s.QuantityOnHand))
            .OrderBy(dto => dto.QuantityOnHand)
            .ToList();
    }
}