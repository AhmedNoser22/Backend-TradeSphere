namespace TradeSphere.Application.Features.SalesOrders.AddLine;

public sealed class AddSalesOrderLineCommandHandler(
    IRepository<SalesOrder> salesOrderRepository,
    IRepository<Product> productRepository,
    IRepository<InventoryStock> inventoryRepository,
    IApplicationDbContext dbContext) : IRequestHandler<AddSalesOrderLineCommand, Result>
{
    public async Task<Result> Handle(AddSalesOrderLineCommand request, CancellationToken cancellationToken)
    {
        var salesOrder = await salesOrderRepository.GetByIdAsync(request.SalesOrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), request.SalesOrderId);

        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.ProductId);

        if (!product.IsActive)
            return Result.Failure("Cannot sell an inactive product.");

        var stock = await inventoryRepository.FirstOrDefaultAsync(new InventoryStockByProductIdSpecification(request.ProductId), cancellationToken);
        if (stock is null || stock.QuantityOnHand < request.Quantity)
            return Result.Failure($"Not enough stock available for this product (available: {stock?.QuantityOnHand ?? 0}).");

        try
        {
            var line = salesOrder.AddLine(request.ProductId, request.Quantity, request.UnitPrice);
            await dbContext.Set<SalesOrderLine>().AddAsync(line, cancellationToken);
        }
        catch (BusinessRuleViolationException ex)
        {
            return Result.Failure(ex.Message);
        }

        return Result.Success();
    }
}