namespace TradeSphere.Domain.Entities;
public sealed class Product : AuditableEntity
{
    public string Sku { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public string Unit { get; private set; } = "Piece"; // e.g. Piece, Box, Kg
    public bool IsActive { get; private set; } = true;

    private Product() { }

    public Product(string sku, string name, string unit, string? description = null)
    {
        Sku = sku;
        Name = name;
        Unit = unit;
        Description = description;
    }

    public void UpdateDetails(string name, string unit, string? description)
    {
        Name = name;
        Unit = unit;
        Description = description;
    }

    public void Deactivate() => IsActive = false;
}