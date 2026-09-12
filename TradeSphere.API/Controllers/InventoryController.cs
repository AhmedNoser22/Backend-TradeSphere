namespace TradeSphere.Api.Controllers;

[RequireRole(UserRole.WarehouseOfficer, UserRole.OperationsManager, UserRole.GeneralManager)]
public sealed class InventoryController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<InventoryStockListItemDto>>> GetList(
        [FromQuery] GetInventoryStocksQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpGet("low-stock")]
    public async Task<ActionResult<List<InventoryStockListItemDto>>> GetLowStock(
        [FromQuery] int threshold, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetLowStockProductsQuery(threshold), cancellationToken));

    [RequireRole(UserRole.WarehouseOfficer, UserRole.OperationsManager)]
    [HttpPost("adjust")]
    public async Task<ActionResult> Adjust(AdjustInventoryStockCommand command, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(command, cancellationToken));
}