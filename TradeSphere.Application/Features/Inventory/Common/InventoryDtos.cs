namespace TradeSphere.Application.Features.Inventory.Common;

public sealed record InventoryStockListItemDto(Guid ProductId, string ProductSku, string ProductName, int QuantityOnHand);