using Microsoft.Extensions.DependencyInjection;

namespace RpgApi.Application;

/// <summary>
/// Registrerar alla Application-tjänster i DI-containern.
///
/// MediatR.AddMediatR() skannar det angivna assemblyt och registrerar
/// automatiskt alla IRequestHandler-implementationer den hittar.
/// Vi slipper registrera varje handler manuellt.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
