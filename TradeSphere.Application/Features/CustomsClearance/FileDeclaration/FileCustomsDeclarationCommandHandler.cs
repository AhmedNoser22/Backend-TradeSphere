namespace TradeSphere.Application.Features.CustomsClearance.FileDeclaration;

public sealed class FileCustomsDeclarationCommandHandler(IRepository<Domain.Entities.CustomsClearance> customsRepository)
    : IRequestHandler<FileCustomsDeclarationCommand, Result>
{
    public async Task<Result> Handle(FileCustomsDeclarationCommand request, CancellationToken cancellationToken)
    {
        var clearance = await customsRepository.GetByIdAsync(request.CustomsClearanceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.CustomsClearance), request.CustomsClearanceId);

        try
        {
            clearance.FileDeclaration(request.DeclarationNumber, request.Port, request.DeclaredGoodsValue);
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        customsRepository.Update(clearance);
        return Result.Success();
    }
}