namespace TradeSphere.Application.Features.Inventory.AdjustStock;

public sealed class AdjustInventoryStockCommandHandler(
    IRepository<InventoryStock> inventoryRepository,
    IRepository<Product> productRepository,
    IApplicationDbContext dbContext) : IRequestHandler<AdjustInventoryStockCommand, Result>
{
    public async Task<Result> Handle(AdjustInventoryStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await inventoryRepository.FirstOrDefaultAsync(new InventoryStockByProductIdSpecification(request.ProductId), cancellationToken);

        if (stock is null)
        {
            if (request.QuantityChange < 0)
                return Result.Failure("No existing stock found for this product.");

            if (!await productRepository.AnyAsync(new ProductByIdSpecification(request.ProductId), cancellationToken))
                throw new NotFoundException(nameof(Product), request.ProductId);

            stock = new InventoryStock(request.ProductId);
            await inventoryRepository.AddAsync(stock, cancellationToken);
        }

        try
        {
            var movement = stock.AdjustQuantity(request.QuantityChange, request.Reason);
            await dbContext.Set<InventoryMovement>().AddAsync(movement, cancellationToken);
        }
        catch (BusinessRuleViolationException ex)
        {
            return Result.Failure(ex.Message);
        }

        return Result.Success();
    }
}