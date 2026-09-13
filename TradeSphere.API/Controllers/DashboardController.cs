namespace TradeSphere.Api.Controllers;

[RequireRole(UserRole.OperationsManager, UserRole.GeneralManager)]
public sealed class DashboardController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary(
        [FromQuery] int lowStockThreshold = 10, [FromQuery] int customsDelayDays = 5, CancellationToken cancellationToken = default) =>
        Ok(await Mediator.Send(new GetDashboardSummaryQuery(lowStockThreshold, customsDelayDays), cancellationToken));
}