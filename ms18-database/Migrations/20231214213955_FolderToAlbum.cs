using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maasgroep.Database.Migrations
{
    /// <inheritdoc />
    public partial class FolderToAlbum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Folders_FolderLocationId",
                schema: "photoAlbum",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "Folders",
                schema: "photoAlbum");

            migrationBuilder.RenameColumn(
                name: "FolderLocationId",
                schema: "photoAlbum",
                table: "Photos",
                newName: "AlbumLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_FolderLocationId",
                schema: "photoAlbum",
                table: "Photos",
                newName: "IX_Photos_AlbumLocationId");

            migrationBuilder.CreateTable(
                name: "albums",
                schema: "photoAlbum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentAlbumId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_albums_albums_ParentAlbumId",
                        column: x => x.ParentAlbumId,
                        principalSchema: "photoAlbum",
                        principalTable: "albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_albums_ParentAlbumId_Name",
                schema: "photoAlbum",
                table: "albums",
                columns: new[] { "ParentAlbumId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_albums_AlbumLocationId",
                schema: "photoAlbum",
                table: "Photos",
                column: "AlbumLocationId",
                principalSchema: "photoAlbum",
                principalTable: "albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_albums_AlbumLocationId",
                schema: "photoAlbum",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "albums",
                schema: "photoAlbum");

            migrationBuilder.RenameColumn(
                name: "AlbumLocationId",
                schema: "photoAlbum",
                table: "Photos",
                newName: "FolderLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_AlbumLocationId",
                schema: "photoAlbum",
                table: "Photos",
                newName: "IX_Photos_FolderLocationId");

            migrationBuilder.CreateTable(
                name: "Folders",
                schema: "photoAlbum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentFolderId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalSchema: "photoAlbum",
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId_Name",
                schema: "photoAlbum",
                table: "Folders",
                columns: new[] { "ParentFolderId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Folders_FolderLocationId",
                schema: "photoAlbum",
                table: "Photos",
                column: "FolderLocationId",
                principalSchema: "photoAlbum",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
