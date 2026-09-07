namespace TradeSphere.Domain.Enums;

// Distinguishes money coming IN from a customer vs money going OUT to a supplier,
// so Finance can track both "customer owes us" and "we owe the supplier" with one entity shape.
public enum PaymentDirection
{
    Incoming = 1, // from customer
    Outgoing = 2  // to supplier
}