using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maasgroep.Database.Migrations
{
    /// <inheritdoc />
    public partial class adminMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_member_Name",
                schema: "admin",
                table: "member");

            migrationBuilder.EnsureSchema(
                name: "adminHistory");

            migrationBuilder.CreateSequence(
                name: "memberSeq",
                schema: "adminHistory");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                schema: "admin",
                table: "member",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "admin",
                table: "member",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "member",
                schema: "adminHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"adminHistory\".\"memberSeq\"')"),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Email = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    MemberPermissions = table.Column<string>(type: "character varying(64000)", maxLength: 64000, nullable: false),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_member", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_member_Email",
                schema: "admin",
                table: "member",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "member",
                schema: "adminHistory");

            migrationBuilder.DropIndex(
                name: "IX_member_Email",
                schema: "admin",
                table: "member");

            migrationBuilder.DropSequence(
                name: "memberSeq",
                schema: "adminHistory");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                schema: "admin",
                table: "member",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "admin",
                table: "member",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_member_Name",
                schema: "admin",
                table: "member",
                column: "Name",
                unique: true);
        }
    }
}
