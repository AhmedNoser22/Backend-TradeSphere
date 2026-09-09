namespace TradeSphere.Application.Features.Users.ActivateUser;

public sealed class ActivateUserCommandHandler(IRepository<User> userRepository) : IRequestHandler<ActivateUserCommand, Result>
{
    public async Task<Result> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        user.Activate();
        userRepository.Update(user);

        return Result.Success();
    }
}