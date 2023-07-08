using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IUsta.Migrations
{
    /// <inheritdoc />
    public partial class addedcategoryinsideWorker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Workers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Workers_CategoryId",
                table: "Workers",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Categories_CategoryId",
                table: "Workers",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Categories_CategoryId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_CategoryId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Workers");
        }
    }
}
