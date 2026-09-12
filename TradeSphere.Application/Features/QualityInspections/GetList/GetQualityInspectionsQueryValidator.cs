namespace TradeSphere.Application.Features.QualityInspections.GetList;

public sealed class GetQualityInspectionsQueryValidator : AbstractValidator<GetQualityInspectionsQuery>
{
    public GetQualityInspectionsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}