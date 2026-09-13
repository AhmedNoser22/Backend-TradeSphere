namespace TradeSphere.Api.Controllers;

[RequireRole(UserRole.QualityControlOfficer, UserRole.OperationsManager, UserRole.GeneralManager)]
public sealed class QualityInspectionsController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<QualityInspectionListItemDto>>> GetList(
        [FromQuery] GetQualityInspectionsQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<QualityInspectionDetailsDto>> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetQualityInspectionByIdQuery(id), cancellationToken));

    [RequireRole(UserRole.QualityControlOfficer, UserRole.OperationsManager)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateQualityInspectionCommand command, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(command, cancellationToken));

    [RequireRole(UserRole.QualityControlOfficer, UserRole.OperationsManager)]
    [HttpPost("{id:guid}/lines")]
    public async Task<ActionResult> RecordLine(Guid id, [FromBody] RecordLineRequest request, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new RecordQualityInspectionLineCommand(id, request.ProductId, request.AcceptedQuantity, request.RejectedQuantity, request.MissingQuantity), cancellationToken));

    [RequireRole(UserRole.QualityControlOfficer, UserRole.OperationsManager)]
    [HttpPost("{id:guid}/complete")]
    public async Task<ActionResult> Complete(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new CompleteQualityInspectionCommand(id), cancellationToken));
}

public sealed record RecordLineRequest(Guid ProductId, int AcceptedQuantity, int RejectedQuantity, int MissingQuantity);