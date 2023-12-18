using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maasgroep.Database.Migrations
{
    /// <inheritdoc />
    public partial class PhotoTagsToAlbumTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Photos_PhotoId",
                schema: "photoAlbum",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_member_MemberId",
                schema: "photoAlbum",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_albums_AlbumLocationId",
                schema: "photoAlbum",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_member_UploaderId",
                schema: "photoAlbum",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "PhotoTags",
                schema: "photoAlbum");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                schema: "photoAlbum",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                schema: "photoAlbum",
                table: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                schema: "photoAlbum",
                table: "Likes");

            migrationBuilder.RenameTable(
                name: "Tags",
                schema: "photoAlbum",
                newName: "tags",
                newSchema: "photoAlbum");

            migrationBuilder.RenameTable(
                name: "Photos",
                schema: "photoAlbum",
                newName: "photos",
                newSchema: "photoAlbum");

            migrationBuilder.RenameTable(
                name: "Likes",
                schema: "photoAlbum",
                newName: "likes",
                newSchema: "photoAlbum");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_Name",
                schema: "photoAlbum",
                table: "tags",
                newName: "IX_tags_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_UploaderId",
                schema: "photoAlbum",
                table: "photos",
                newName: "IX_photos_UploaderId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_AlbumLocationId",
                schema: "photoAlbum",
                table: "photos",
                newName: "IX_photos_AlbumLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_PhotoId",
                schema: "photoAlbum",
                table: "likes",
                newName: "IX_likes_PhotoId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_MemberId",
                schema: "photoAlbum",
                table: "likes",
                newName: "IX_likes_MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tags",
                schema: "photoAlbum",
                table: "tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_photos",
                schema: "photoAlbum",
                table: "photos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_likes",
                schema: "photoAlbum",
                table: "likes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "albumTags",
                schema: "photoAlbum",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albumTags", x => new { x.AlbumId, x.TagId });
                    table.ForeignKey(
                        name: "FK_albumTags_albums_AlbumId",
                        column: x => x.AlbumId,
                        principalSchema: "photoAlbum",
                        principalTable: "albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_albumTags_tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "photoAlbum",
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_albumTags_TagId",
                schema: "photoAlbum",
                table: "albumTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_likes_member_MemberId",
                schema: "photoAlbum",
                table: "likes",
                column: "MemberId",
                principalSchema: "admin",
                principalTable: "member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_likes_photos_PhotoId",
                schema: "photoAlbum",
                table: "likes",
                column: "PhotoId",
                principalSchema: "photoAlbum",
                principalTable: "photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_photos_albums_AlbumLocationId",
                schema: "photoAlbum",
                table: "photos",
                column: "AlbumLocationId",
                principalSchema: "photoAlbum",
                principalTable: "albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_photos_member_UploaderId",
                schema: "photoAlbum",
                table: "photos",
                column: "UploaderId",
                principalSchema: "admin",
                principalTable: "member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_likes_member_MemberId",
                schema: "photoAlbum",
                table: "likes");

            migrationBuilder.DropForeignKey(
                name: "FK_likes_photos_PhotoId",
                schema: "photoAlbum",
                table: "likes");

            migrationBuilder.DropForeignKey(
                name: "FK_photos_albums_AlbumLocationId",
                schema: "photoAlbum",
                table: "photos");

            migrationBuilder.DropForeignKey(
                name: "FK_photos_member_UploaderId",
                schema: "photoAlbum",
                table: "photos");

            migrationBuilder.DropTable(
                name: "albumTags",
                schema: "photoAlbum");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tags",
                schema: "photoAlbum",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_photos",
                schema: "photoAlbum",
                table: "photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_likes",
                schema: "photoAlbum",
                table: "likes");

            migrationBuilder.RenameTable(
                name: "tags",
                schema: "photoAlbum",
                newName: "Tags",
                newSchema: "photoAlbum");

            migrationBuilder.RenameTable(
                name: "photos",
                schema: "photoAlbum",
                newName: "Photos",
                newSchema: "photoAlbum");

            migrationBuilder.RenameTable(
                name: "likes",
                schema: "photoAlbum",
                newName: "Likes",
                newSchema: "photoAlbum");

            migrationBuilder.RenameIndex(
                name: "IX_tags_Name",
                schema: "photoAlbum",
                table: "Tags",
                newName: "IX_Tags_Name");

            migrationBuilder.RenameIndex(
                name: "IX_photos_UploaderId",
                schema: "photoAlbum",
                table: "Photos",
                newName: "IX_Photos_UploaderId");

            migrationBuilder.RenameIndex(
                name: "IX_photos_AlbumLocationId",
                schema: "photoAlbum",
                table: "Photos",
                newName: "IX_Photos_AlbumLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_likes_PhotoId",
                schema: "photoAlbum",
                table: "Likes",
                newName: "IX_Likes_PhotoId");

            migrationBuilder.RenameIndex(
                name: "IX_likes_MemberId",
                schema: "photoAlbum",
                table: "Likes",
                newName: "IX_Likes_MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                schema: "photoAlbum",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                schema: "photoAlbum",
                table: "Photos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                schema: "photoAlbum",
                table: "Likes",
                column: "Id");

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
                name: "IX_PhotoTags_TagId",
                schema: "photoAlbum",
                table: "PhotoTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Photos_PhotoId",
                schema: "photoAlbum",
                table: "Likes",
                column: "PhotoId",
                principalSchema: "photoAlbum",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_member_MemberId",
                schema: "photoAlbum",
                table: "Likes",
                column: "MemberId",
                principalSchema: "admin",
                principalTable: "member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_albums_AlbumLocationId",
                schema: "photoAlbum",
                table: "Photos",
                column: "AlbumLocationId",
                principalSchema: "photoAlbum",
                principalTable: "albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_member_UploaderId",
                schema: "photoAlbum",
                table: "Photos",
                column: "UploaderId",
                principalSchema: "admin",
                principalTable: "member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
