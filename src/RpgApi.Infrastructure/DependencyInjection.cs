using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RpgApi.Application.Common.Interfaces;
using RpgApi.Domain.Interfaces;
using RpgApi.Infrastructure.Persistence;
using RpgApi.Infrastructure.Repositories;
using RpgApi.Infrastructure.Services;

namespace RpgApi.Infrastructure;

/// <summary>
/// Extension method för IServiceCollection – ett vanligt mönster i .NET
/// för att gruppera relaterade DI-registreringar.
/// 
/// Fördelar:
/// - Program.cs hålls ren och minimal
/// - Infrastructure-lagret "äger" sin egen konfiguration
/// - Enkelt att hitta och ändra registreringar
/// 
/// 'static class' + extension methods = inga instanser behövs,
/// metoden anropas som om den vore en metod på IServiceCollection.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Anslutningssträng 'DefaultConnection' saknas i konfigurationen.");

        services.AddDbContext<RpgDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                // Retry-logik för transient fel (nätverksproblem, SQL Server restart).
                // EF Core försöker automatiskt igen vid dessa fel.
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });

        // Scoped = en instans per HTTP-request.
        // Det är rätt livstid för DbContext och repositories –
        // de ska leva lika länge som requesten och sedan kastas.
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

        return services;
    }
}
