namespace TradeSphere.Application.Features.PurchaseOrders.Cancel;

public sealed class CancelPurchaseOrderCommandHandler(IRepository<PurchaseOrder> purchaseOrderRepository) : IRequestHandler<CancelPurchaseOrderCommand, Result>
{
    public async Task<Result> Handle(CancelPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(PurchaseOrder), request.PurchaseOrderId);

        try
        {
            purchaseOrder.Cancel();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        purchaseOrderRepository.Update(purchaseOrder);
        return Result.Success();
    }
}