using TradeSphere.Domain.ValueObjects;

namespace TradeSphere.Domain.Entities;
public sealed class Customer : AuditableEntity
{
    public string Name { get; private set; } = default!;
    public ContactInfo Contact { get; private set; } = default!;
    public Address? BillingAddress { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Customer() { }

    public Customer(string name, ContactInfo contact, Address? billingAddress = null)
    {
        Name = name;
        Contact = contact;
        BillingAddress = billingAddress;
    }

    public void UpdateDetails(string name, ContactInfo contact, Address? billingAddress)
    {
        Name = name;
        Contact = contact;
        BillingAddress = billingAddress;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}