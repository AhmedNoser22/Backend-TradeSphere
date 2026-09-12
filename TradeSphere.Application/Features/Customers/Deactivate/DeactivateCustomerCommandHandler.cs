namespace TradeSphere.Application.Features.Customers.Deactivate;

public sealed class DeactivateCustomerCommandHandler(IRepository<Customer> customerRepository) : IRequestHandler<DeactivateCustomerCommand, Result>
{
    public async Task<Result> Handle(DeactivateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        customer.Deactivate();
        customerRepository.Update(customer);
        return Result.Success();
    }
}