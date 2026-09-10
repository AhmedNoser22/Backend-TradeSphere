namespace TradeSphere.Application.Features.PurchaseOrders.Confirm;

public sealed class ConfirmPurchaseOrderCommandHandler(IRepository<PurchaseOrder> purchaseOrderRepository) : IRequestHandler<ConfirmPurchaseOrderCommand, Result>
{
    public async Task<Result> Handle(ConfirmPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await purchaseOrderRepository.FirstOrDefaultAsync(
            new PurchaseOrderByIdWithLinesSpecification(request.PurchaseOrderId), cancellationToken)
            ?? throw new NotFoundException(nameof(PurchaseOrder), request.PurchaseOrderId);

        try
        {
            purchaseOrder.Confirm();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        purchaseOrderRepository.Update(purchaseOrder);
        return Result.Success();
    }
}