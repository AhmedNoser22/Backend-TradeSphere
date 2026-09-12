namespace TradeSphere.Api.Controllers;

[RequireRole(UserRole.SalesOfficer, UserRole.OperationsManager, UserRole.GeneralManager)]
public sealed class CustomersController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<CustomerListItemDto>>> GetList(
        [FromQuery] GetCustomersQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerDetailsDto>> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetCustomerByIdQuery(id), cancellationToken));

    [RequireRole(UserRole.SalesOfficer, UserRole.OperationsManager)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateCustomerCommand command, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(command, cancellationToken));

    [RequireRole(UserRole.SalesOfficer, UserRole.OperationsManager)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new UpdateCustomerCommand(id, request.Name, request.Email, request.Phone, request.Country, request.City, request.Street, request.PostalCode), cancellationToken));

    [RequireRole(UserRole.SalesOfficer, UserRole.OperationsManager)]
    [HttpPost("{id:guid}/deactivate")]
    public async Task<ActionResult> Deactivate(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new DeactivateCustomerCommand(id), cancellationToken));

    [RequireRole(UserRole.SalesOfficer, UserRole.OperationsManager)]
    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult> Activate(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new ActivateCustomerCommand(id), cancellationToken));
}

public sealed record UpdateCustomerRequest(string Name, string Email, string? Phone, string? Country, string? City, string? Street, string? PostalCode);