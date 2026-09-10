namespace TradeSphere.Application.Features.Shipments.GetList;

public sealed class GetShipmentsQueryValidator : AbstractValidator<GetShipmentsQuery>
{
    public GetShipmentsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}