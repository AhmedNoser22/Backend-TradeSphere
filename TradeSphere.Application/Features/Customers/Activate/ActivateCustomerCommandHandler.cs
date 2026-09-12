namespace TradeSphere.Application.Features.Customers.Activate;

public sealed class ActivateCustomerCommandHandler(IRepository<Customer> customerRepository) : IRequestHandler<ActivateCustomerCommand, Result>
{
    public async Task<Result> Handle(ActivateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        customer.Activate();
        customerRepository.Update(customer);
        return Result.Success();
    }
}