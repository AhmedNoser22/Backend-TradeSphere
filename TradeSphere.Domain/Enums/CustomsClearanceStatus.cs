namespace TradeSphere.Domain.Enums;

public enum CustomsClearanceStatus
{
    PendingDocuments = 1,   // goods physically arrived but paperwork not filed yet
    UnderClearance = 2,     // filed, waiting for customs authority
    Cleared = 3,            // released by customs — still not in warehouse, QC is next
    Rejected = 4
}