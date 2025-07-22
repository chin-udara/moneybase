using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneybaseChat.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTeamIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "identifier",
                table: "team",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_team_identifier",
                table: "team",
                column: "identifier",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_team_identifier",
                table: "team");

            migrationBuilder.DropColumn(
                name: "identifier",
                table: "team");
        }
    }
}
