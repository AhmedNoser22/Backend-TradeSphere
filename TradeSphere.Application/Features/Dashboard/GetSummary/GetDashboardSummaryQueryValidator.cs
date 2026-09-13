namespace TradeSphere.Application.Features.Dashboard.GetSummary;

public sealed class GetDashboardSummaryQueryValidator : AbstractValidator<GetDashboardSummaryQuery>
{
    public GetDashboardSummaryQueryValidator()
    {
        RuleFor(x => x.LowStockThreshold).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CustomsDelayDays).GreaterThanOrEqualTo(0);
    }
}