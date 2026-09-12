namespace TradeSphere.Application.Features.Users.GetById;

public sealed class GetUserByIdQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetUserByIdQuery, UserListItemDto>
{
    public async Task<UserListItemDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await dbContext.Users
            .Where(u => u.Id == request.UserId)
            .ProjectToType<UserListItemDto>()
            .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(User), request.UserId);
    }
}