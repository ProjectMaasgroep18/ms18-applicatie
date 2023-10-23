﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms18_applicatie.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "receipt");

            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.EnsureSchema(
                name: "photo");

            migrationBuilder.CreateSequence(
                name: "costCentreSeq",
                schema: "receipt");

            migrationBuilder.CreateSequence(
                name: "memberSeq",
                schema: "admin");

            migrationBuilder.CreateSequence(
                name: "permissionSeq",
                schema: "admin");

            migrationBuilder.CreateSequence(
                name: "PhotoSeq",
                schema: "photo");

            migrationBuilder.CreateSequence(
                name: "receiptSeq",
                schema: "receipt");

            migrationBuilder.CreateSequence(
                name: "receiptStatusSeq",
                schema: "receipt");

            migrationBuilder.CreateSequence(
                name: "storeSeq",
                schema: "receipt");

            migrationBuilder.CreateTable(
                name: "member",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('admin.\"memberSeq\"')"),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserCreatedId = table.Column<long>(type: "bigint", nullable: true),
                    UserModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_member_memberCreated",
                        column: x => x.UserCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_member_memberModified",
                        column: x => x.UserModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "costCentre",
                schema: "receipt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('receipt.\"costCentreSeq\"')"),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    UserModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_costCentre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_costCentre_memberCreated",
                        column: x => x.UserCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_costCentre_memberModified",
                        column: x => x.UserModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "permission",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('admin.\"permissionSeq\"')"),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    UserModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_permission_memberCreated",
                        column: x => x.UserCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_permission_memberModified",
                        column: x => x.UserModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "status",
                schema: "receipt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('receipt.\"receiptStatusSeq\"')"),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    UserModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_receiptStatus_memberCreated",
                        column: x => x.UserCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receiptStatus_memberModified",
                        column: x => x.UserModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "store",
                schema: "receipt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('receipt.\"storeSeq\"')"),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    UserModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_store", x => x.Id);
                    table.ForeignKey(
                        name: "FK_store_memberCreated",
                        column: x => x.UserCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_store_memberModified",
                        column: x => x.UserModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "memberPermission",
                schema: "admin",
                columns: table => new
                {
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false),
                    UserCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    UserModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_memberPermission", x => new { x.MemberId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_memberPermission_member",
                        column: x => x.MemberId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_memberPermission_memberCreated",
                        column: x => x.UserCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_memberPermission_memberModified",
                        column: x => x.UserModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_memberPermission_permission",
                        column: x => x.PermissionId,
                        principalSchema: "admin",
                        principalTable: "permission",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "receipt",
                schema: "receipt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('receipt.\"receiptSeq\"')"),
                    Amount = table.Column<decimal>(type: "numeric(2)", precision: 2, nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    CostCentreId = table.Column<long>(type: "bigint", nullable: true),
                    ReceiptStatusId = table.Column<long>(type: "bigint", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    UserCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    UserModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_receipt_costCentre",
                        column: x => x.CostCentreId,
                        principalSchema: "receipt",
                        principalTable: "costCentre",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receipt_memberCreated",
                        column: x => x.UserCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receipt_memberModified",
                        column: x => x.UserModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receipt_receiptStatus",
                        column: x => x.ReceiptStatusId,
                        principalSchema: "receipt",
                        principalTable: "status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receipt_store",
                        column: x => x.StoreId,
                        principalSchema: "receipt",
                        principalTable: "store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "approval",
                schema: "receipt",
                columns: table => new
                {
                    ReceiptId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    UserCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    UserModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval", x => x.ReceiptId);
                    table.ForeignKey(
                        name: "FK_receiptApproval_memberCreated",
                        column: x => x.UserCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receiptApproval_memberModified",
                        column: x => x.UserModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receiptApproval_receipt",
                        column: x => x.ReceiptId,
                        principalSchema: "receipt",
                        principalTable: "receipt",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "photo",
                schema: "photo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('photo.\"PhotoSeq\"')"),
                    Receipt = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    Bytes = table.Column<byte[]>(type: "bytea", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_photo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photo_Receipt",
                        column: x => x.Receipt,
                        principalSchema: "receipt",
                        principalTable: "receipt",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_approval_UserCreatedId",
                schema: "receipt",
                table: "approval",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_approval_UserModifiedId",
                schema: "receipt",
                table: "approval",
                column: "UserModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_costCentre_Name",
                schema: "receipt",
                table: "costCentre",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_costCentre_UserCreatedId",
                schema: "receipt",
                table: "costCentre",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_costCentre_UserModifiedId",
                schema: "receipt",
                table: "costCentre",
                column: "UserModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_member_Name",
                schema: "admin",
                table: "member",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_member_UserCreatedId",
                schema: "admin",
                table: "member",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_member_UserModifiedId",
                schema: "admin",
                table: "member",
                column: "UserModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_memberPermission_PermissionId",
                schema: "admin",
                table: "memberPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_memberPermission_UserCreatedId",
                schema: "admin",
                table: "memberPermission",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_memberPermission_UserModifiedId",
                schema: "admin",
                table: "memberPermission",
                column: "UserModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_Name",
                schema: "admin",
                table: "permission",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permission_UserCreatedId",
                schema: "admin",
                table: "permission",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_UserModifiedId",
                schema: "admin",
                table: "permission",
                column: "UserModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_photo_Receipt",
                schema: "photo",
                table: "photo",
                column: "Receipt",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_receipt_CostCentreId",
                schema: "receipt",
                table: "receipt",
                column: "CostCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_ReceiptStatusId",
                schema: "receipt",
                table: "receipt",
                column: "ReceiptStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_StoreId",
                schema: "receipt",
                table: "receipt",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_UserCreatedId",
                schema: "receipt",
                table: "receipt",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_UserModifiedId",
                schema: "receipt",
                table: "receipt",
                column: "UserModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_status_Name",
                schema: "receipt",
                table: "status",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_status_UserCreatedId",
                schema: "receipt",
                table: "status",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_status_UserModifiedId",
                schema: "receipt",
                table: "status",
                column: "UserModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_store_Name",
                schema: "receipt",
                table: "store",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_store_UserCreatedId",
                schema: "receipt",
                table: "store",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_store_UserModifiedId",
                schema: "receipt",
                table: "store",
                column: "UserModifiedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "approval",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "memberPermission",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "photo",
                schema: "photo");

            migrationBuilder.DropTable(
                name: "permission",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "receipt",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "costCentre",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "status",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "store",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "member",
                schema: "admin");

            migrationBuilder.DropSequence(
                name: "costCentreSeq",
                schema: "receipt");

            migrationBuilder.DropSequence(
                name: "memberSeq",
                schema: "admin");

            migrationBuilder.DropSequence(
                name: "permissionSeq",
                schema: "admin");

            migrationBuilder.DropSequence(
                name: "PhotoSeq",
                schema: "photo");

            migrationBuilder.DropSequence(
                name: "receiptSeq",
                schema: "receipt");

            migrationBuilder.DropSequence(
                name: "receiptStatusSeq",
                schema: "receipt");

            migrationBuilder.DropSequence(
                name: "storeSeq",
                schema: "receipt");
        }
    }
}