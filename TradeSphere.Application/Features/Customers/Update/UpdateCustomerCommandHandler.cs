namespace TradeSphere.Application.Features.Customers.Update;

public sealed class UpdateCustomerCommandHandler(IRepository<Customer> customerRepository) : IRequestHandler<UpdateCustomerCommand, Result>
{
    public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        var contact = new ContactInfo(request.Email, request.Phone);
        var address = request.Country is null ? null : new Address(request.Country, request.City ?? "", request.Street ?? "", request.PostalCode);

        customer.UpdateDetails(request.Name, contact, address);
        customerRepository.Update(customer);

        return Result.Success();
    }
}