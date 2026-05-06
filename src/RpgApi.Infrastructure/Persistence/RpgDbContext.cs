using Microsoft.EntityFrameworkCore;
using RpgApi.Domain.Entities;

namespace RpgApi.Infrastructure.Persistence;

/// <summary>
/// DbContext är EF Cores centrala klass – den representerar en session
/// mot databasen och fungerar som ett Unit of Work + Identity Map.
/// 
/// Identity Map: EF Core håller koll på alla laddade entiteter.
/// Om du hämtar samma karaktär två gånger i samma scope, returneras
/// SAMMA C#-objekt (inte två kopior). Ändringar i objektet spåras automatiskt.
/// 
/// partial class: tillåter oss att dela upp klassen i flera filer om den växer.
/// </summary>
public partial class RpgDbContext : DbContext
{
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<CharacterItem> CharacterItems => Set<CharacterItem>();

    // Konstruktorn tar DbContextOptions – det gör att vi kan konfigurera
    // anslutningssträngen utifrån (i Program.cs) utan att hårdkoda den.
    public RpgDbContext(DbContextOptions<RpgDbContext> options) : base(options) { }

    /// <summary>
    /// OnModelCreating konfigurerar mappningen mellan C#-klasser och databastabeller.
    /// Vi delegerar till separata IEntityTypeConfiguration-klasser (se Configurations/)
    /// för att hålla denna metod ren. Single Responsibility Principle!
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Applicera alla konfigurationer i detta assembly automatiskt.
        // Slipper registrera varje konfigurationsklass manuellt.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RpgDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
