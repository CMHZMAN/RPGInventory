using RpgApi.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace RpgApi.Api.Middleware;

/// <summary>
/// Global exception-hanterare som fångar ALLA ohanterade undantag
/// och konverterar dem till strukturerade HTTP-svar.
///
/// Varför middleware istället för try/catch i varje controller?
/// - DRY (Don't Repeat Yourself) – ett ställe för all felhantering
/// - Controllers förblir rena och fokuserar på happy path
/// - Konsekvent JSON-felformat för alla endpoints
///
/// Middleware-kedjan: Request → [denna] → [nästa middleware] → Controller
/// Vid undantag: Controller kastar → bubblar upp → fångas här → JSON-svar
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Anropa nästa middleware i kedjan
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ohanterat undantag: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var isDevelopment = context.RequestServices
            .GetRequiredService<IWebHostEnvironment>().IsDevelopment();

        // Pattern matching på undantagstyp → rätt HTTP-statuskod
        var (statusCode, message) = exception switch
        {
            NotFoundException nfe       => (HttpStatusCode.NotFound, nfe.Message),
            DomainException de          => (HttpStatusCode.BadRequest, de.Message),
            ArgumentException ae        => (HttpStatusCode.BadRequest, ae.Message),
            InvalidOperationException i => (HttpStatusCode.Conflict, i.Message),
            // I Development: visa faktiskt felmeddelande + stack trace för enklare felsökning
            _ when isDevelopment        => (HttpStatusCode.InternalServerError,
                                            $"{exception.Message}\n{exception.InnerException?.Message}\n{exception.StackTrace}"),
            _                           => (HttpStatusCode.InternalServerError,
                                            "Ett internt serverfel inträffade.")
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = (int)statusCode,
            error = message
        };

        // camelCase JSON – standard för REST API:er som konsumeras av JavaScript
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
