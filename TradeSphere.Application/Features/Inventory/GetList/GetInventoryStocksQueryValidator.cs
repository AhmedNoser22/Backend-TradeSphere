namespace TradeSphere.Application.Features.Inventory.GetList;

public sealed class GetInventoryStocksQueryValidator : AbstractValidator<GetInventoryStocksQuery>
{
    public GetInventoryStocksQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}