namespace TradeSphere.Application.Features.SalesOrders.Deliver;

public sealed class DeliverSalesOrderCommandValidator : AbstractValidator<DeliverSalesOrderCommand>
{
    public DeliverSalesOrderCommandValidator() => RuleFor(x => x.SalesOrderId).NotEmpty();
}