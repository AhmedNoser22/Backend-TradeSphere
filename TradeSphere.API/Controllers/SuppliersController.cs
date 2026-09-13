namespace TradeSphere.Api.Controllers;

public sealed class SuppliersController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<SupplierDto>>> GetSuppliers(
        [FromQuery] GetSuppliersQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SupplierDto>> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetSupplierByIdQuery(id), cancellationToken));

    // Write endpoints: only the roles that actually own supplier data.
    [RequireRole(UserRole.ProcurementOfficer)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateSupplierCommand command, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(command, cancellationToken));

    [RequireRole(UserRole.ProcurementOfficer)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateSupplierRequest request, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new UpdateSupplierCommand(id, request.Name, request.Country, request.Email, request.Phone), cancellationToken));

    [RequireRole(UserRole.ProcurementOfficer)]
    [HttpPost("{id:guid}/deactivate")]
    public async Task<ActionResult> Deactivate(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new DeactivateSupplierCommand(id), cancellationToken));

    [RequireRole(UserRole.ProcurementOfficer)]
    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult> Activate(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new ActivateSupplierCommand(id), cancellationToken));
}

// Body shape for Update — keeps the Id coming from the route, not duplicated in the body.
public sealed record UpdateSupplierRequest(string Name, string Country, string Email, string? Phone);