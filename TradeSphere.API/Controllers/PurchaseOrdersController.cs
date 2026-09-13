namespace TradeSphere.Api.Controllers;
[RequireRole(UserRole.ProcurementOfficer, UserRole.OperationsManager, UserRole.GeneralManager)]
public sealed class PurchaseOrdersController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<PurchaseOrderListItemDto>>> GetList(
        [FromQuery] GetPurchaseOrdersQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PurchaseOrderDetailsDto>> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetPurchaseOrderByIdQuery(id), cancellationToken));

    [RequireRole(UserRole.ProcurementOfficer, UserRole.OperationsManager)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreatePurchaseOrderCommand command, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(command, cancellationToken));

    [RequireRole(UserRole.ProcurementOfficer, UserRole.OperationsManager)]
    [HttpPost("{id:guid}/lines")]
    public async Task<ActionResult> AddLine(Guid id, [FromBody] AddLineRequest request, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new AddPurchaseOrderLineCommand(id, request.ProductId, request.Quantity, request.UnitPrice, request.Currency), cancellationToken));

    [RequireRole(UserRole.ProcurementOfficer, UserRole.OperationsManager)]
    [HttpPost("{id:guid}/confirm")]
    public async Task<ActionResult> Confirm(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new ConfirmPurchaseOrderCommand(id), cancellationToken));

    [RequireRole(UserRole.ProcurementOfficer, UserRole.OperationsManager)]
    [HttpPost("{id:guid}/cancel")]
    public async Task<ActionResult> Cancel(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new CancelPurchaseOrderCommand(id), cancellationToken));
}

public sealed record AddLineRequest(Guid ProductId, int Quantity, decimal UnitPrice, string Currency);