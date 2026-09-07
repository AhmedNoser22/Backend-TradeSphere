using TradeSphere.Infrastructure.Identity;

namespace TradeSphere.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, SmtpEmailService>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IBackgroundJobsService, HangfireBackgroundJobsService>();

        var filesRootPath = configuration["FileStorage:RootPath"] ?? "App_Data/files";
        services.AddScoped<IFileStorageService>(_ => new LocalFileStorageService(filesRootPath));

        // Auth: password hashing + JWT generation
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IPasswordHasher, PasswordHasherService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}