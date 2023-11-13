using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maasgroep.Database.Migrations
{
    /// <inheritdoc />
    public partial class photoBase64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Base64Image",
                schema: "photo",
                table: "photo",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Base64Image",
                schema: "photo",
                table: "photo");
        }
    }
}
