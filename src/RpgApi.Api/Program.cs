using RpgApi.Api.Middleware;
using RpgApi.Application;
using RpgApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --- Tjänsteregistrering ---
// Varje lager registrerar sina egna tjänster via extension methods.
// Program.cs hålls tunn och behöver inte känna till implementationsdetaljer.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// CORS – tillåter React-appen (localhost:5173 = Vite default) att anropa API:et
builder.Services.AddCors(options =>
{
    options.AddPolicy("RpgFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// --- Middleware-pipeline ---
// Ordningen spelar roll! Exception-hanteraren måste vara FÖRST
// så att den fångar fel från alla efterföljande middlewares.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("RpgFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
