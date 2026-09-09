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

        user.Deactivate();
        userRepository.Update(user);

        return Result.Success();
    }
}