namespace TradeSphere.Application.Features.Users.ChangeUserRole;

public sealed class ChangeUserRoleCommandHandler(
    IRepository<User> userRepository,
    ICurrentUserService currentUserService) : IRequestHandler<ChangeUserRoleCommand, Result>
{
    public async Task<Result> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == currentUserService.UserId)
            return Result.Failure("You cannot change your own role.");

        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        user.ChangeRole(request.NewRole);
        userRepository.Update(user);

        return Result.Success();
    }
}