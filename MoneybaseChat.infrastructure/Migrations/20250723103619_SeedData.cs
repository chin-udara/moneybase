using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoneybaseChat.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "team",
                columns: new[] { "id", "identifier", "is_overflow_team", "name", "shift_duration_minutes", "shift_start_time" },
                values: new object[,]
                {
                    { 1, new Guid("74f42bf3-376c-4ee2-bfc1-37471f883638"), false, "Team A", 480, new TimeSpan(0, 0, 0, 0, 0) },
                    { 2, new Guid("c5a0f4c2-336c-424e-abb3-2aa09e5de3c4"), false, "Team B", 480, new TimeSpan(0, 8, 0, 0, 0) },
                    { 3, new Guid("85f3cd9b-b74b-4e39-8f86-51b634ebfc78"), false, "Team C", 480, new TimeSpan(0, 16, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "agent",
                columns: new[] { "id", "name", "seniority", "team" },
                values: new object[,]
                {
                    { 1, "Erica", 3, 1 },
                    { 2, "James", 2, 1 },
                    { 3, "Michelle", 1, 2 },
                    { 4, "Tim", 0, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "agent",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "agent",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "agent",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "agent",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 3);
        }
    }
}
