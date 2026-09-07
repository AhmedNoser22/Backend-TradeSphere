namespace TradeSphere.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(200).IsRequired();

        builder.OwnsOne(c => c.Contact, contact =>
        {
            contact.Property(x => x.Email).HasColumnName("Email").HasMaxLength(200).IsRequired();
            contact.Property(x => x.Phone).HasColumnName("Phone").HasMaxLength(50);
        });

        builder.OwnsOne(c => c.BillingAddress, address =>
        {
            address.Property(x => x.Country).HasColumnName("BillingCountry").HasMaxLength(100);
            address.Property(x => x.City).HasColumnName("BillingCity").HasMaxLength(100);
            address.Property(x => x.Street).HasColumnName("BillingStreet").HasMaxLength(200);
            address.Property(x => x.PostalCode).HasColumnName("BillingPostalCode").HasMaxLength(20);
        });
    }
}