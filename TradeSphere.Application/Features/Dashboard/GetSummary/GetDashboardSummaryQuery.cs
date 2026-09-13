namespace TradeSphere.Application.Features.Dashboard.GetSummary;
public sealed record GetDashboardSummaryQuery(int LowStockThreshold = 10, int CustomsDelayDays = 5) : IRequest<DashboardSummaryDto>;