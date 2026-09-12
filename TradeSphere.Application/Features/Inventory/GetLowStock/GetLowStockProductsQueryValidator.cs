namespace TradeSphere.Application.Features.Inventory.GetLowStock;
public sealed class GetLowStockProductsQueryValidator : AbstractValidator<GetLowStockProductsQuery>
{
    public GetLowStockProductsQueryValidator() => RuleFor(x => x.Threshold).GreaterThanOrEqualTo(0);
}