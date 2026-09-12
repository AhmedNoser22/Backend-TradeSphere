namespace TradeSphere.Application.Features.SalesOrders.AddLine;

public sealed class AddSalesOrderLineCommandValidator : AbstractValidator<AddSalesOrderLineCommand>
{
    public AddSalesOrderLineCommandValidator()
    {
        RuleFor(x => x.SalesOrderId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
    }
}