using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IUsta.Migrations
{
    /// <inheritdoc />
    public partial class removedupdatedat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Reservations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Reservations",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
