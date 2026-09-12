namespace TradeSphere.Application.Features.Customers.Activate;

public sealed record ActivateCustomerCommand(Guid CustomerId) : IRequest<Result>, ITransactionalRequest;