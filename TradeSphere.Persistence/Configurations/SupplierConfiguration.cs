namespace TradeSphere.Persistence.Configurations;

public sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Country).HasMaxLength(100).IsRequired();

        // ContactInfo is a Value Object with no Id of its own, so it's stored
        // as two plain columns on the Suppliers table (owned type), not a separate table.
        builder.OwnsOne(s => s.Contact, contact =>
        {
            contact.Property(c => c.Email).HasColumnName("Email").HasMaxLength(200).IsRequired();
            contact.Property(c => c.Phone).HasColumnName("Phone").HasMaxLength(50);
        });

        builder.HasMany(s => s.PurchaseOrders)
            .WithOne(po => po.Supplier)
            .HasForeignKey(po => po.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}