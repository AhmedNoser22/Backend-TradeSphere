namespace TradeSphere.Application.Features.SalesOrders.Cancel;

public sealed class CancelSalesOrderCommandValidator : AbstractValidator<CancelSalesOrderCommand>
{
    public CancelSalesOrderCommandValidator() => RuleFor(x => x.SalesOrderId).NotEmpty();
}