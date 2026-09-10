namespace TradeSphere.Application.Features.PurchaseOrders.Cancel;
public sealed class CancelPurchaseOrderCommandValidator : AbstractValidator<CancelPurchaseOrderCommand>
{
    public CancelPurchaseOrderCommandValidator() => RuleFor(x => x.PurchaseOrderId).NotEmpty();
}