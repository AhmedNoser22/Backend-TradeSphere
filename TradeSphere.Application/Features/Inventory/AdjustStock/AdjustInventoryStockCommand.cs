namespace TradeSphere.Application.Features.Inventory.AdjustStock;

public sealed record AdjustInventoryStockCommand(Guid ProductId, int QuantityChange, string Reason) : IRequest<Result>, ITransactionalRequest;