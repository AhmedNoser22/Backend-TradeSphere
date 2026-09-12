namespace TradeSphere.Api.Controllers;

[RequireRole(UserRole.SalesOfficer, UserRole.OperationsManager, UserRole.GeneralManager)]
public sealed class SalesOrdersController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<SalesOrderListItemDto>>> GetList(
        [FromQuery] GetSalesOrdersQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SalesOrderDetailsDto>> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetSalesOrderByIdQuery(id), cancellationToken));

    [RequireRole(UserRole.SalesOfficer, UserRole.OperationsManager)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateSalesOrderCommand command, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(command, cancellationToken));

    [RequireRole(UserRole.SalesOfficer, UserRole.OperationsManager)]
    [HttpPost("{id:guid}/lines")]
    public async Task<ActionResult> AddLine(Guid id, [FromBody] AddSalesLineRequest request, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new AddSalesOrderLineCommand(id, request.ProductId, request.Quantity, request.UnitPrice), cancellationToken));

    [RequireRole(UserRole.SalesOfficer, UserRole.OperationsManager)]
    [HttpPost("{id:guid}/confirm")]
    public async Task<ActionResult> Confirm(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new ConfirmSalesOrderCommand(id), cancellationToken));

    [RequireRole(UserRole.SalesOfficer, UserRole.OperationsManager, UserRole.WarehouseOfficer)]
    [HttpPost("{id:guid}/deliver")]
    public async Task<ActionResult> Deliver(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new DeliverSalesOrderCommand(id), cancellationToken));

    [RequireRole(UserRole.SalesOfficer, UserRole.OperationsManager)]
    [HttpPost("{id:guid}/cancel")]
    public async Task<ActionResult> Cancel(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new CancelSalesOrderCommand(id), cancellationToken));
}

public sealed record AddSalesLineRequest(Guid ProductId, int Quantity, decimal UnitPrice);