namespace TradeSphere.Application.Features.Customers.Create;

public sealed record CreateCustomerCommand(
    string Name, string Email, string? Phone, string? Country, string? City, string? Street, string? PostalCode)
    : IRequest<Result<Guid>>, ITransactionalRequest;