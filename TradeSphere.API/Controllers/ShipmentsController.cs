namespace TradeSphere.Api.Controllers;

[RequireRole(UserRole.LogisticsOfficer, UserRole.OperationsManager, UserRole.GeneralManager, UserRole.SystemAdministrator)]
public sealed class ShipmentsController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ShipmentListItemDto>>> GetList(
        [FromQuery] GetShipmentsQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [RequireRole(UserRole.LogisticsOfficer, UserRole.OperationsManager,UserRole.SystemAdministrator)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateShipmentCommand command, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(command, cancellationToken));

    [RequireRole(UserRole.LogisticsOfficer, UserRole.OperationsManager,UserRole.SystemAdministrator)]
    [HttpPost("{id:guid}/mark-shipped")]
    public async Task<ActionResult> MarkAsShipped(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new MarkShipmentAsShippedCommand(id), cancellationToken));

    [RequireRole(UserRole.LogisticsOfficer, UserRole.OperationsManager, UserRole.SystemAdministrator)]
    [HttpPost("{id:guid}/mark-in-transit")]
    public async Task<ActionResult> MarkAsInTransit(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new MarkShipmentAsInTransitCommand(id), cancellationToken));

    [RequireRole(UserRole.LogisticsOfficer, UserRole.OperationsManager, UserRole.SystemAdministrator)]
    [HttpPost("{id:guid}/mark-arrived")]
    public async Task<ActionResult> MarkAsArrived(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new MarkShipmentAsArrivedCommand(id), cancellationToken));
}