namespace TradeSphere.Application.Features.SalesOrders.Confirm;

public sealed class ConfirmSalesOrderCommandValidator : AbstractValidator<ConfirmSalesOrderCommand>
{
    public ConfirmSalesOrderCommandValidator() => RuleFor(x => x.SalesOrderId).NotEmpty();
}