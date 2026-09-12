namespace TradeSphere.Application.Features.Users.DeactivateUser;

public sealed class DeactivateUserCommandHandler(
    IRepository<User> userRepository,
    ICurrentUserService currentUserService) : IRequestHandler<DeactivateUserCommand, Result>
{
    public async Task<Result> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == currentUserService.UserId)
            return Result.Failure("You cannot deactivate your own account.");

        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        if (user.Role == UserRole.SystemAdministrator
            && !await userRepository.AnyAsync(new OtherActiveAdminExistsSpecification(user.Id), cancellationToken))
            return Result.Failure("Cannot deactivate the last active System Administrator.");

        user.Deactivate();
        userRepository.Update(user);
        return Result.Success();
    }
}