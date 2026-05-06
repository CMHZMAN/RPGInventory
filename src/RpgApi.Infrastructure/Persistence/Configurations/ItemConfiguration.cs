using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RpgApi.Domain.Entities;

namespace RpgApi.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("nvarchar(100)");

        builder.Property(i => i.Description)
            .HasMaxLength(500)
            .HasColumnType("nvarchar(500)");

        builder.Property(i => i.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(i => i.StrengthBonus).HasDefaultValue(0);
        builder.Property(i => i.IntelligenceBonus).HasDefaultValue(0);
        builder.Property(i => i.AgilityBonus).HasDefaultValue(0);
        builder.Property(i => i.DefenseBonus).HasDefaultValue(0);

        builder.HasIndex(i => i.Type);
    }
}
