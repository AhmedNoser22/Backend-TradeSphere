namespace TradeSphere.Application.Features.Customers.Create;

public sealed class CreateCustomerCommandHandler(IRepository<Customer> customerRepository) : IRequestHandler<CreateCustomerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var contact = new ContactInfo(request.Email, request.Phone);
        var address = request.Country is null ? null : new Address(request.Country, request.City ?? "", request.Street ?? "", request.PostalCode);

        var customer = new Customer(request.Name, contact, address);
        await customerRepository.AddAsync(customer, cancellationToken);

        return Result<Guid>.Success(customer.Id);
    }
}