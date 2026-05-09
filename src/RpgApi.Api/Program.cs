<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
<<<<<<< HEAD
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
=======
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
using RpgApi.Api.Middleware;
using RpgApi.Application;
using RpgApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
<<<<<<< HEAD
// --- Tjänsteregistrering ---
// Varje lager registrerar sina egna tjänster via extension methods.
// Program.cs hålls tunn och behöver inte känna till implementationsdetaljer.
=======
// ── Tjänsteregistrering ────────────────────────────────────────
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
<<<<<<< HEAD
builder.Services.AddOpenApi();
=======
// ── Tjänsteregistrering ────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

// ── JWT-autentisering ──────────────────────────────────────────
// Läser konfiguration från appsettings.json → Jwt-sektionen.
// AddAuthentication berättar vilket scheme som är default.
// AddJwtBearer konfigurerar hur tokens valideras vid varje request.
var jwtSection = builder.Configuration.GetSection("Jwt");
var secretKey   = jwtSection["Secret"]
    ?? throw new InvalidOperationException("Jwt:Secret saknas i konfigurationen.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Vad ska valideras i varje token?
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,   // Kontrollera att token inte löpt ut
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSection["Issuer"],
            ValidAudience            = jwtSection["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(secretKey)),
            // ClockSkew: tillåt 0 sekunders tidsskillnad (default är 5 min)
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

// ── Swagger / OpenAPI ──────────────────────────────────────────
// Swashbuckle genererar ett swagger.json-dokument från controllers
// och tillhandahåller Swagger UI för interaktiv testning.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "⚔ RPG API",
        Version     = "v1",
        Description = "Backend-API för RPG-systemet. Använd /api/auth/register sedan /api/auth/login för att få en JWT-token.",
    });

    // Definiera JWT-säkerhetsschema i Swagger UI
    // Användaren kan klicka "Authorize" och klistra in sin token
    var jwtScheme = new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Klistra in din JWT-token här (utan 'Bearer'-prefixet).",
        Reference    = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id   = JwtBearerDefaults.AuthenticationScheme,
        }
    };

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtScheme);

    // Kräv JWT för alla endpoints som standard
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtScheme, Array.Empty<string>() }
    });

    // Inkludera XML-kommentarer (/// <summary>...) från Api-projektet
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// ── CORS ───────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("RpgFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
=======
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API

// ── JWT-autentisering ──────────────────────────────────────────
// Läser konfiguration från appsettings.json → Jwt-sektionen.
// AddAuthentication berättar vilket scheme som är default.
// AddJwtBearer konfigurerar hur tokens valideras vid varje request.
var jwtSection = builder.Configuration.GetSection("Jwt");
var secretKey   = jwtSection["Secret"]
    ?? throw new InvalidOperationException("Jwt:Secret saknas i konfigurationen.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Vad ska valideras i varje token?
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,   // Kontrollera att token inte löpt ut
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSection["Issuer"],
            ValidAudience            = jwtSection["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(secretKey)),
            // ClockSkew: tillåt 0 sekunders tidsskillnad (default är 5 min)
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

// ── Swagger / OpenAPI ──────────────────────────────────────────
// Swashbuckle genererar ett swagger.json-dokument från controllers
// och tillhandahåller Swagger UI för interaktiv testning.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "⚔ RPG API",
        Version     = "v1",
        Description = "Backend-API för RPG-systemet. Använd /api/auth/register sedan /api/auth/login för att få en JWT-token.",
    });

    // Definiera JWT-säkerhetsschema i Swagger UI
    // Användaren kan klicka "Authorize" och klistra in sin token
    var jwtScheme = new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Klistra in din JWT-token här (utan 'Bearer'-prefixet).",
        Reference    = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id   = JwtBearerDefaults.AuthenticationScheme,
        }
    };

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtScheme);

    // Kräv JWT för alla endpoints som standard
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtScheme, Array.Empty<string>() }
    });

    // Inkludera XML-kommentarer (/// <summary>...) från Api-projektet
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// ── CORS ───────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("RpgFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

<<<<<<< HEAD
<<<<<<< HEAD
// --- Middleware-pipeline ---
// Ordningen spelar roll! Exception-hanteraren måste vara FÖRST
// så att den fångar fel från alla efterföljande middlewares.
=======
// ── Middleware-pipeline ────────────────────────────────────────
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
=======
// ── Middleware-pipeline ────────────────────────────────────────
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Swagger UI tillgänglig på /swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RPG API v1");
        c.RoutePrefix = "swagger"; // http://localhost:5115/swagger
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseCors("RpgFrontend");
<<<<<<< HEAD
<<<<<<< HEAD
=======
app.UseAuthentication(); // ← Måste komma FÖRE UseAuthorization
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
=======
app.UseAuthentication(); // ← Måste komma FÖRE UseAuthorization
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
app.UseAuthorization();
app.MapControllers();

app.Run();
