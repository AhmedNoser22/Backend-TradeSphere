namespace TradeSphere.Application.Features.Customers.Deactivate;

public sealed record DeactivateCustomerCommand(Guid CustomerId) : IRequest<Result>, ITransactionalRequest;