namespace TradeSphere.Application.Features.Customers.GetById;

public sealed class GetCustomerByIdQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetCustomerByIdQuery, CustomerDetailsDto>
{
    public async Task<CustomerDetailsDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await dbContext.Customers
            .Where(c => c.Id == request.CustomerId)
            .Select(c => new CustomerDetailsDto(
                c.Id, c.Name, c.Contact.Email, c.Contact.Phone,
                c.BillingAddress!.Country, c.BillingAddress!.City, c.BillingAddress!.Street, c.BillingAddress!.PostalCode, c.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(Customer), request.CustomerId);
    }
}