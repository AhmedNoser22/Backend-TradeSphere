namespace TradeSphere.Application.Features.CustomsClearance.GetList;

public sealed class GetCustomsClearancesQueryValidator : AbstractValidator<GetCustomsClearancesQuery>
{
    public GetCustomsClearancesQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}