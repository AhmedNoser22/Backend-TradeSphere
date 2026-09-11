namespace TradeSphere.Application.Features.CustomsClearance.FileDeclaration;

public sealed record FileCustomsDeclarationCommand(
    Guid CustomsClearanceId, string DeclarationNumber, string Port, decimal DeclaredGoodsValue) : IRequest<Result>, ITransactionalRequest;