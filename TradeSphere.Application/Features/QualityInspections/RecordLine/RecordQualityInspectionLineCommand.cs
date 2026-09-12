namespace TradeSphere.Application.Features.QualityInspections.RecordLine;

public sealed record RecordQualityInspectionLineCommand(
    Guid QualityInspectionId, Guid ProductId, int AcceptedQuantity, int RejectedQuantity, int MissingQuantity) : IRequest<Result>, ITransactionalRequest;