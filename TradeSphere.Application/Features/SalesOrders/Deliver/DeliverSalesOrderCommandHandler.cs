namespace TradeSphere.Application.Features.SalesOrders.Deliver;

public sealed class DeliverSalesOrderCommandHandler(
    IRepository<SalesOrder> salesOrderRepository,
    IRepository<InventoryStock> inventoryRepository,
    IApplicationDbContext dbContext) : IRequestHandler<DeliverSalesOrderCommand, Result>
{
    public async Task<Result> Handle(DeliverSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var salesOrder = await salesOrderRepository.FirstOrDefaultAsync(
            new SalesOrderByIdWithLinesSpecification(request.SalesOrderId), cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), request.SalesOrderId);

        var productIds = salesOrder.Lines.Select(l => l.ProductId).ToList();
        var stocks = await inventoryRepository.ListAsync(new InventoryStocksByProductIdsSpecification(productIds), cancellationToken);
        var stockByProduct = stocks.ToDictionary(s => s.ProductId);

        foreach (var line in salesOrder.Lines)
        {
            if (!stockByProduct.TryGetValue(line.ProductId, out var stock) || stock.QuantityOnHand < line.Quantity)
                return Result.Failure($"Not enough stock to deliver product {line.ProductId}.");
        }

        try
        {
            foreach (var line in salesOrder.Lines)
            {
                var movement = stockByProduct[line.ProductId].IssueForSale(line.Quantity, salesOrder.Id);
                await dbContext.Set<InventoryMovement>().AddAsync(movement, cancellationToken);
            }

            salesOrder.MarkAsDelivered();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        return Result.Success();
    }
}