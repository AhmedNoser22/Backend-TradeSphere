namespace TradeSphere.Application.Features.Suppliers.Common;

public static class SupplierMapping
{
    public static void Register()
    {
        TypeAdapterConfig<Supplier, SupplierDto>
            .NewConfig()
            .Map(dest => dest.Email, src => src.Contact.Email)
            .Map(dest => dest.Phone, src => src.Contact.Phone);
    }
}