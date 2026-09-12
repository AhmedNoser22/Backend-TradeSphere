namespace TradeSphere.Application.Features.SalesOrders.GetList;

public sealed class GetSalesOrdersQueryValidator : AbstractValidator<GetSalesOrdersQuery>
{
    public GetSalesOrdersQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}