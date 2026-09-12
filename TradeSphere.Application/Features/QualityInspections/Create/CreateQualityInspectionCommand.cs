namespace TradeSphere.Application.Features.QualityInspections.Create;

public sealed record CreateQualityInspectionCommand(Guid CustomsClearanceId) : IRequest<Result<Guid>>, ITransactionalRequest;