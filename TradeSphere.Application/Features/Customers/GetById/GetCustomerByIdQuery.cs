namespace TradeSphere.Application.Features.Customers.GetById;

public sealed record GetCustomerByIdQuery(Guid CustomerId) : IRequest<CustomerDetailsDto>;