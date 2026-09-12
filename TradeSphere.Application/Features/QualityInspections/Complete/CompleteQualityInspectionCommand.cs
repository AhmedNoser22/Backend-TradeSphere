namespace TradeSphere.Application.Features.QualityInspections.Complete;

public sealed record CompleteQualityInspectionCommand(Guid QualityInspectionId) : IRequest<Result>, ITransactionalRequest;