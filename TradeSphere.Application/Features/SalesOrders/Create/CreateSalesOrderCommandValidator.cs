namespace TradeSphere.Application.Features.SalesOrders.Create;

public sealed class CreateSalesOrderCommandValidator : AbstractValidator<CreateSalesOrderCommand>
{
    public CreateSalesOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.OrderDate).NotEmpty();
    }
}