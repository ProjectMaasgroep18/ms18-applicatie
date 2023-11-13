using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maasgroep.Database.Migrations
{
    /// <inheritdoc />
    public partial class photoCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Receipt",
                schema: "photo",
                table: "photo");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Receipt",
                schema: "photo",
                table: "photo",
                column: "Receipt",
                principalSchema: "receipt",
                principalTable: "receipt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Receipt",
                schema: "photo",
                table: "photo");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Receipt",
                schema: "photo",
                table: "photo",
                column: "Receipt",
                principalSchema: "receipt",
                principalTable: "receipt",
                principalColumn: "Id");
        }
    }
}
