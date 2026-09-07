namespace TradeSphere.Application.Common.Behaviors;
public interface ITransactionalRequest;

public sealed class TransactionBehavior<TRequest, TResponse>(IApplicationDbContext dbContext) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is ITransactionalRequest)
            await dbContext.SaveChangesAsync(cancellationToken);

        return response;
    }
}