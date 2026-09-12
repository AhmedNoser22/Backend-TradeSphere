namespace TradeSphere.Application.Features.Inventory.GetLowStock;

public sealed record GetLowStockProductsQuery(int Threshold = 10) : IRequest<List<InventoryStockListItemDto>>;