namespace RpgApi.Application.Common.Exceptions;

/// <summary>
/// Kastas när en efterfrågad resurs inte hittas i databasen.
/// Fångas i API-lagret och översätts till HTTP 404 Not Found.
///
/// Egna undantagstyper är bättre än generiska Exception eftersom:
/// - Koden kommunicerar AVSIKTEN (NotFoundException vs Exception)
/// - Middleware kan fånga specifika typer och returnera rätt HTTP-statuskod
/// - Loggar blir tydligare
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string entityName, Guid id)
        : base($"{entityName} med id '{id}' hittades inte.") { }
}

/// <summary>
/// Kastas när affärsregler bryts på applikationsnivå.
/// Översätts till HTTP 400 Bad Request eller 409 Conflict.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
