namespace TradeSphere.Api.Controllers;

[RequireRole(UserRole.FinanceOfficer, UserRole.OperationsManager, UserRole.GeneralManager)]
public sealed class PaymentsController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<PaymentListItemDto>>> GetList(
        [FromQuery] GetPaymentsQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PaymentListItemDto>> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetPaymentByIdQuery(id), cancellationToken));

    [RequireRole(UserRole.FinanceOfficer, UserRole.OperationsManager)]
    [HttpPost("{id:guid}/record-installment")]
    public async Task<ActionResult> RecordInstallment(Guid id, [FromBody] RecordInstallmentRequest request, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new RecordPaymentInstallmentCommand(id, request.Amount), cancellationToken));
}

public sealed record RecordInstallmentRequest(decimal Amount);