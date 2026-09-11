namespace TradeSphere.Api.Controllers;

[Route("api/customs-clearances")]
[RequireRole(UserRole.CustomsClearanceOfficer, UserRole.OperationsManager, UserRole.GeneralManager,UserRole.SystemAdministrator)]
public sealed class CustomsClearancesController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<CustomsClearanceListItemDto>>> GetList(
        [FromQuery] GetCustomsClearancesQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [RequireRole(UserRole.CustomsClearanceOfficer, UserRole.OperationsManager, UserRole.SystemAdministrator)]
    [HttpPost("{id:guid}/file-declaration")]
    public async Task<ActionResult> FileDeclaration(Guid id, [FromBody] FileDeclarationRequest request, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new FileCustomsDeclarationCommand(id, request.DeclarationNumber, request.Port, request.DeclaredGoodsValue), cancellationToken));

    [RequireRole(UserRole.CustomsClearanceOfficer, UserRole.OperationsManager, UserRole.SystemAdministrator)]
    [HttpPost("{id:guid}/clear")]
    public async Task<ActionResult> Clear(Guid id, [FromBody] ClearRequest request, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new ClearCustomsCommand(id, request.CustomsDuties, request.ClearanceFees), cancellationToken));

    [RequireRole(UserRole.CustomsClearanceOfficer, UserRole.OperationsManager, UserRole.SystemAdministrator)]
    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult> Reject(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new RejectCustomsCommand(id), cancellationToken));
}

public sealed record FileDeclarationRequest(string DeclarationNumber, string Port, decimal DeclaredGoodsValue);
public sealed record ClearRequest(decimal CustomsDuties, decimal ClearanceFees);