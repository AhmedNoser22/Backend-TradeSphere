using TradeSphere.Domain.ValueObjects;

namespace TradeSphere.Domain.Entities;
public sealed class Supplier : AuditableEntity
{
    public string Name { get; private set; } = default!;
    public string Country { get; private set; } = default!;
    public ContactInfo Contact { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    private readonly List<PurchaseOrder> _purchaseOrders = [];
    public IReadOnlyCollection<PurchaseOrder> PurchaseOrders => _purchaseOrders.AsReadOnly();

    private Supplier() { } // for EF Core

    public Supplier(string name, string country, ContactInfo contact)
    {
        Name = name;
        Country = country;
        Contact = contact;
    }

    public void Deactivate() => IsActive = false;
    public void Reactivate() => IsActive = true;

    public void UpdateDetails(string name, string country, ContactInfo contact)
    {
        Name = name;
        Country = country;
        Contact = contact;
    }
}