using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maasgroep.Database.Migrations
{
    /// <inheritdoc />
    public partial class OneToManyReceiptPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_photo_Receipt",
                schema: "photo",
                table: "photo");

            migrationBuilder.CreateIndex(
                name: "IX_photo_Receipt",
                schema: "photo",
                table: "photo",
                column: "Receipt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_photo_Receipt",
                schema: "photo",
                table: "photo");

            migrationBuilder.CreateIndex(
                name: "IX_photo_Receipt",
                schema: "photo",
                table: "photo",
                column: "Receipt",
                unique: true);
        }
    }
}
