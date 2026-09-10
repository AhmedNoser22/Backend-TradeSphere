namespace TradeSphere.Application.Features.PurchaseOrders.Confirm;

public sealed class ConfirmPurchaseOrderCommandValidator : AbstractValidator<ConfirmPurchaseOrderCommand>
{
    public ConfirmPurchaseOrderCommandValidator() => RuleFor(x => x.PurchaseOrderId).NotEmpty();
}