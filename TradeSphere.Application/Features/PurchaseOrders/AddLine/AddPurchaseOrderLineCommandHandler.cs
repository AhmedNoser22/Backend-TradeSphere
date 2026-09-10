namespace TradeSphere.Application.Features.PurchaseOrders.AddLine;

public sealed class AddPurchaseOrderLineCommandHandler(
    IRepository<PurchaseOrder> purchaseOrderRepository,
    IRepository<Product> productRepository) : IRequestHandler<AddPurchaseOrderLineCommand, Result>
{
    public async Task<Result> Handle(AddPurchaseOrderLineCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(PurchaseOrder), request.PurchaseOrderId);

        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.ProductId);

        if (!product.IsActive)
            return Result.Failure("Cannot add an inactive product to a purchase order.");

        try
        {
            purchaseOrder.AddLine(request.ProductId, request.Quantity, request.UnitPrice, request.Currency);
        }
        catch (BusinessRuleViolationException ex)
        {
            return Result.Failure(ex.Message);
        }

        purchaseOrderRepository.Update(purchaseOrder);
        return Result.Success();
    }
}