namespace TradeSphere.Application.Features.Suppliers.GetSupplierById;

public sealed class GetSupplierByIdQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetSupplierByIdQuery, SupplierDto>
{
    public async Task<SupplierDto> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await dbContext.Suppliers
            .Where(s => s.Id == request.Id)
            .ProjectToType<SupplierDto>()
            .FirstOrDefaultAsync(cancellationToken);

        return supplier ?? throw new NotFoundException(nameof(Supplier), request.Id);
    }
}