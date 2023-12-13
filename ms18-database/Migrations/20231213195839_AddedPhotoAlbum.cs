using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maasgroep.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedPhotoAlbum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "photoAlbum");

            migrationBuilder.CreateTable(
                name: "Folders",
                schema: "photoAlbum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentFolderId = table.Column<Guid>(type: "uuid", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "photoAlbum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                schema: "photoAlbum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UploaderId = table.Column<long>(type: "bigint", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ImageData = table.Column<byte[]>(type: "bytea", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    FolderLocationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Folders_FolderLocationId",
                        column: x => x.FolderLocationId,
                        principalSchema: "photoAlbum",
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Photos_member_UploaderId",
                        column: x => x.UploaderId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                schema: "photoAlbum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    PhotoId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalSchema: "photoAlbum",
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoTags",
                schema: "photoAlbum",
                columns: table => new
                {
                    PhotoId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoTags", x => new { x.PhotoId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PhotoTags_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalSchema: "photoAlbum",
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "photoAlbum",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId_Name",
                schema: "photoAlbum",
                table: "Folders",
                columns: new[] { "ParentFolderId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_MemberId",
                schema: "photoAlbum",
                table: "Likes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_PhotoId",
                schema: "photoAlbum",
                table: "Likes",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_FolderLocationId",
                schema: "photoAlbum",
                table: "Photos",
                column: "FolderLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UploaderId",
                schema: "photoAlbum",
                table: "Photos",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoTags_TagId",
                schema: "photoAlbum",
                table: "PhotoTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                schema: "photoAlbum",
                table: "Tags",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes",
                schema: "photoAlbum");

            migrationBuilder.DropTable(
                name: "PhotoTags",
                schema: "photoAlbum");

            migrationBuilder.DropTable(
                name: "Photos",
                schema: "photoAlbum");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "photoAlbum");

            migrationBuilder.DropTable(
                name: "Folders",
                schema: "photoAlbum");
        }
    }
}
