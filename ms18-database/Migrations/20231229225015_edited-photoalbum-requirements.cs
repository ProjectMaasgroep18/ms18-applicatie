using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maasgroep.Database.Migrations
{
    /// <inheritdoc />
    public partial class editedphotoalbumrequirements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_photos_albums_AlbumLocationId",
                schema: "photoAlbum",
                table: "photos");

            migrationBuilder.DropForeignKey(
                name: "FK_photos_member_UploaderId",
                schema: "photoAlbum",
                table: "photos");

            migrationBuilder.AlterColumn<long>(
                name: "UploaderId",
                schema: "photoAlbum",
                table: "photos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "AlbumLocationId",
                schema: "photoAlbum",
                table: "photos",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_photos_albums_AlbumLocationId",
                schema: "photoAlbum",
                table: "photos",
                column: "AlbumLocationId",
                principalSchema: "photoAlbum",
                principalTable: "albums",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_photos_member_UploaderId",
                schema: "photoAlbum",
                table: "photos",
                column: "UploaderId",
                principalSchema: "admin",
                principalTable: "member",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_photos_albums_AlbumLocationId",
                schema: "photoAlbum",
                table: "photos");

            migrationBuilder.DropForeignKey(
                name: "FK_photos_member_UploaderId",
                schema: "photoAlbum",
                table: "photos");

            migrationBuilder.AlterColumn<long>(
                name: "UploaderId",
                schema: "photoAlbum",
                table: "photos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AlbumLocationId",
                schema: "photoAlbum",
                table: "photos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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
    }
}
