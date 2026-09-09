namespace TradeSphere.Api.Controllers;

[RequireRole(UserRole.SystemAdministrator)]
public sealed class UsersController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<UserListItemDto>>> GetUsers(
        [FromQuery] GetUsersQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpPost("{id:guid}/deactivate")]
    public async Task<ActionResult> Deactivate(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new DeactivateUserCommand(id), cancellationToken));

    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult> Activate(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new ActivateUserCommand(id), cancellationToken));

    [HttpPut("{id:guid}/role")]
    public async Task<ActionResult> ChangeRole(Guid id, [FromBody] ChangeRoleRequest request, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new ChangeUserRoleCommand(id, request.NewRole), cancellationToken));
}

public sealed record ChangeRoleRequest(UserRole NewRole);