namespace TradeSphere.Api.Controllers;

[RequireRole(UserRole.SystemAdministrator)]
public sealed class UsersController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<UserListItemDto>>> GetUsers(
        [FromQuery] GetUsersQuery query, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserListItemDto>> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetUserByIdQuery(id), cancellationToken));

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateUserByAdminCommand command, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(command, cancellationToken));

    [HttpPut("{id:guid}/role")]
    public async Task<ActionResult> ChangeRole(Guid id, [FromBody] ChangeRoleRequest request, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new ChangeUserRoleCommand(id, request.NewRole), cancellationToken));

    [HttpPost("{id:guid}/deactivate")]
    public async Task<ActionResult> Deactivate(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new DeactivateUserCommand(id), cancellationToken));

    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult> Activate(Guid id, CancellationToken cancellationToken) =>
        HandleResult(await Mediator.Send(new ActivateUserCommand(id), cancellationToken));
}

public sealed record ChangeRoleRequest(UserRole NewRole);