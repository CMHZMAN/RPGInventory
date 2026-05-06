using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "Name", "StrengthBonus", "Type" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000001"), "Ett vanligt järnsvärd.", "Järnsvärd", 5, 1 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "IntelligenceBonus", "Name", "Type" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000002"), "En stav laddad med magisk energi.", 8, "Trollstav", 1 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "AgilityBonus", "DefenseBonus", "Description", "Name", "Type" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000003"), 2, 5, "Lätt rustning som inte hämmar rörelseförmågan.", "Läderharnesk", 2 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "DefenseBonus", "Description", "Name", "StrengthBonus", "Type" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000004"), 12, "Tung och skyddande stålrustning.", "Plåtrustning", 2, 2 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "Name", "Type" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000005"), "Återställer 50 hälsopoäng.", "Helbredelsedryck", 3 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "Name", "StrengthBonus", "Type" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000006"), "Ger +10 styrka i 1 timme.", "Styrkedryck", 10, 3 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "AgilityBonus", "Description", "Name", "StrengthBonus", "Type" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000007"), 4, "En lätt dolk perfekt för lönnmördare.", "Smygdolk", 3, 1 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "IntelligenceBonus", "Name", "Type" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000008"), "Ökar bärarens intelligens.", 6, "Visdomsamulett", 4 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000008"));
        }
    }
}
