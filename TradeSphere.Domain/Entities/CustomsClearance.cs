namespace TradeSphere.Domain.Entities;
public sealed class CustomsClearance : AuditableEntity
{
    public Guid ShipmentId { get; private set; }
    public string? DeclarationNumber { get; private set; }
    public string? Port { get; private set; }
    public decimal DeclaredGoodsValue { get; private set; }
    public decimal CustomsDuties { get; private set; }
    public decimal ClearanceFees { get; private set; }
    public CustomsClearanceStatus Status { get; private set; } = CustomsClearanceStatus.PendingDocuments;

    public decimal TotalLandedCost => DeclaredGoodsValue + CustomsDuties + ClearanceFees;

    private CustomsClearance() { }

    public CustomsClearance(Guid shipmentId) => ShipmentId = shipmentId;

    public void FileDeclaration(string declarationNumber, string port, decimal declaredGoodsValue)
    {
        if (Status != CustomsClearanceStatus.PendingDocuments)
            throw new InvalidStateTransitionException(nameof(CustomsClearance), Status.ToString(), CustomsClearanceStatus.UnderClearance.ToString());

        DeclarationNumber = declarationNumber;
        Port = port;
        DeclaredGoodsValue = declaredGoodsValue;
        Status = CustomsClearanceStatus.UnderClearance;
    }

    public void Clear(decimal customsDuties, decimal clearanceFees)
    {
        if (Status != CustomsClearanceStatus.UnderClearance)
            throw new InvalidStateTransitionException(nameof(CustomsClearance), Status.ToString(), CustomsClearanceStatus.Cleared.ToString());

        CustomsDuties = customsDuties;
        ClearanceFees = clearanceFees;
        Status = CustomsClearanceStatus.Cleared;
    }

    public void Reject()
    {
        if (Status != CustomsClearanceStatus.UnderClearance)
            throw new InvalidStateTransitionException(nameof(CustomsClearance), Status.ToString(), CustomsClearanceStatus.Rejected.ToString());

        Status = CustomsClearanceStatus.Rejected;
    }
}