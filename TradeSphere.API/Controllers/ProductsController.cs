namespace TradeSphere.Api.Controllers;

public sealed class ProductsController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProductDto>>> GetProducts(
        [FromQuery] GetProductsQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetProductByIdQuery(id), cancellationToken));

    [RequireRole(UserRole.ProcurementOfficer, UserRole.SystemAdministrator)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateProductCommand command, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(command, cancellationToken));

    [RequireRole(UserRole.ProcurementOfficer, UserRole.SystemAdministrator)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateProductRequest request, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new UpdateProductCommand(id, request.Name, request.Unit, request.Description), cancellationToken));

    [RequireRole(UserRole.ProcurementOfficer, UserRole.SystemAdministrator)]
    [HttpPost("{id:guid}/deactivate")]
    public async Task<ActionResult> Deactivate(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new DeactivateProductCommand(id), cancellationToken));

    [RequireRole(UserRole.ProcurementOfficer, UserRole.SystemAdministrator)]
    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult> Activate(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new ActivateProductCommand(id), cancellationToken));
}

public sealed record UpdateProductRequest(string Name, string Unit, string? Description);