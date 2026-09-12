namespace TradeSphere.Application.Features.Customers.Update;

public sealed record UpdateCustomerCommand(
    Guid CustomerId, string Name, string Email, string? Phone, string? Country, string? City, string? Street, string? PostalCode)
    : IRequest<Result>, ITransactionalRequest;