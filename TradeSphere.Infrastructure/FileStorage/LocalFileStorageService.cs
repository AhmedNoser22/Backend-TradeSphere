namespace TradeSphere.Infrastructure.FileStorage;
public sealed class LocalFileStorageService(string rootPath) : IFileStorageService
{
    public async Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(rootPath);
        var safeFileName = $"{Guid.NewGuid()}_{fileName}";
        var fullPath = Path.Combine(rootPath, safeFileName);

        await using var output = File.Create(fullPath);
        await fileStream.CopyToAsync(output, cancellationToken);

        return safeFileName;
    }
}