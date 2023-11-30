using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Maasgroep.Database.Migrations
{
    /// <inheritdoc />
    public partial class _000_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "receipt");

            migrationBuilder.EnsureSchema(
                name: "receiptHistory");

            migrationBuilder.EnsureSchema(
                name: "order");

            migrationBuilder.EnsureSchema(
                name: "orderHistory");

            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.EnsureSchema(
                name: "todo");

            migrationBuilder.EnsureSchema(
                name: "todoHistory");

            migrationBuilder.CreateSequence(
                name: "approvalSeq",
                schema: "receiptHistory");

            migrationBuilder.CreateSequence(
                name: "billSeq",
                schema: "orderHistory");

            migrationBuilder.CreateSequence(
                name: "costCentreSeq",
                schema: "receipt");

            migrationBuilder.CreateSequence(
                name: "costCentreSeq",
                schema: "receiptHistory");

            migrationBuilder.CreateSequence(
                name: "lineSeq",
                schema: "orderHistory");

            migrationBuilder.CreateSequence(
                name: "memberSeq",
                schema: "admin");

            migrationBuilder.CreateSequence(
                name: "permissionSeq",
                schema: "admin");

            migrationBuilder.CreateSequence(
                name: "photoSeq",
                schema: "receipt");

            migrationBuilder.CreateSequence(
                name: "photoSeq",
                schema: "receiptHistory");

            migrationBuilder.CreateSequence(
                name: "productPriceSeq",
                schema: "orderHistory");

            migrationBuilder.CreateSequence(
                name: "productSeq",
                schema: "order");

            migrationBuilder.CreateSequence(
                name: "productSeq",
                schema: "orderHistory");

            migrationBuilder.CreateSequence(
                name: "receiptSeq",
                schema: "receipt");

            migrationBuilder.CreateSequence(
                name: "receiptSeq",
                schema: "receiptHistory");

            migrationBuilder.CreateSequence(
                name: "stockSeq",
                schema: "orderHistory");

            migrationBuilder.CreateSequence(
                name: "todoSeq",
                schema: "todo");

            migrationBuilder.CreateSequence(
                name: "todoSeq",
                schema: "todoHistory");

            migrationBuilder.CreateTable(
                name: "approval",
                schema: "receiptHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"receiptHistory\".\"approvalSeq\"')"),
                    ReceiptId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bill",
                schema: "orderHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"orderHistory\".\"billSeq\"')"),
                    BillId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: true),
                    IsGuest = table.Column<bool>(type: "boolean", nullable: false),
                    Note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Name = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "costCentre",
                schema: "receiptHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"receiptHistory\".\"costCentreSeq\"')"),
                    CostCentreId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_costCentre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "line",
                schema: "orderHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"orderHistory\".\"lineSeq\"')"),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: true),
                    IsGuest = table.Column<bool>(type: "boolean", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_line", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "member",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('admin.\"memberSeq\"')"),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_member_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_member_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_member_memberModified",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "photo",
                schema: "receiptHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"receiptHistory\".\"photoSeq\"')"),
                    PhotoId = table.Column<long>(type: "bigint", nullable: false),
                    ReceiptId = table.Column<long>(type: "bigint", nullable: false),
                    Base64Image = table.Column<string>(type: "text", nullable: false),
                    fileExtension = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    fileName = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_photo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "orderHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"orderHistory\".\"productSeq\"')"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "productPrice",
                schema: "orderHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"orderHistory\".\"productPriceSeq\"')"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productPrice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "receipt",
                schema: "receiptHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"receiptHistory\".\"receiptSeq\"')"),
                    ReceiptId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    CostCentreId = table.Column<long>(type: "bigint", nullable: true),
                    ReceiptStatus = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stock",
                schema: "orderHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"orderHistory\".\"stockSeq\"')"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "todo",
                schema: "todoHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"todoHistory\".\"todoSeq\"')"),
                    ToDoId = table.Column<long>(type: "bigint", nullable: false),
                    Action = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    Done = table.Column<bool>(type: "boolean", nullable: false),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bill",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MemberId = table.Column<long>(type: "bigint", nullable: true),
                    IsGuest = table.Column<bool>(type: "boolean", nullable: false),
                    Note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Name = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orderBill_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderBill_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderBill_memberModified",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderBill_memberOwned",
                        column: x => x.MemberId,
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
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_costCentre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_costCentre_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_costCentre_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_costCentre_memberModified",
                        column: x => x.MemberModifiedId,
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
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_permission_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_permission_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_permission_memberModified",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('order.\"productSeq\"')"),
                    Name = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orderProduct_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderProduct_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderProduct_memberModified",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "todo",
                schema: "todo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('todo.\"todoSeq\"')"),
                    Action = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    Done = table.Column<bool>(type: "boolean", nullable: false),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_todo_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_todo_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_todo_memberModified",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_todo_memberOwned",
                        column: x => x.MemberId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "receipt",
                schema: "receipt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('receipt.\"receiptSeq\"')"),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    CostCentreId = table.Column<long>(type: "bigint", nullable: true),
                    ReceiptStatus = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt", x => x.Id);
                    table.CheckConstraint("CK_receipt_amount", "\"Amount\" >= 0");
                    table.ForeignKey(
                        name: "FK_receipt_costCentre",
                        column: x => x.CostCentreId,
                        principalSchema: "receipt",
                        principalTable: "costCentre",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receipt_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receipt_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receipt_memberModified",
                        column: x => x.MemberModifiedId,
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
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_memberPermission_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_memberPermission_memberModified",
                        column: x => x.MemberModifiedId,
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
                name: "line",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BillId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_line", x => x.Id);
                    table.CheckConstraint("CK_orderLine_quantity", "\"Quantity\" > 0");
                    table.ForeignKey(
                        name: "FK_orderLine_bill",
                        column: x => x.BillId,
                        principalSchema: "order",
                        principalTable: "bill",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderLine_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderLine_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderLine_memberModified",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderLine_product",
                        column: x => x.ProductId,
                        principalSchema: "order",
                        principalTable: "product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "productPrice",
                schema: "order",
                columns: table => new
                {
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productPrice", x => x.ProductId);
                    table.CheckConstraint("CK_orderProductPrice_price", "\"Price\" >= 0");
                    table.ForeignKey(
                        name: "FK_orderProductPrice_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderProductPrice_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderProductPrice_memberModified",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderProductPrice_product",
                        column: x => x.ProductId,
                        principalSchema: "order",
                        principalTable: "product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "stock",
                schema: "order",
                columns: table => new
                {
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock", x => x.ProductId);
                    table.CheckConstraint("CK_order_quantity", "\"Quantity\" >= 0");
                    table.ForeignKey(
                        name: "FK_orderStock_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderStock_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderStock_memberModified",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderStock_product",
                        column: x => x.ProductId,
                        principalSchema: "order",
                        principalTable: "product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "approval",
                schema: "receipt",
                columns: table => new
                {
                    ReceiptId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval", x => x.ReceiptId);
                    table.ForeignKey(
                        name: "FK_receiptApproval_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receiptApproval_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receiptApproval_memberModified",
                        column: x => x.MemberModifiedId,
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
                schema: "receipt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('receipt.\"photoSeq\"')"),
                    ReceiptId = table.Column<long>(type: "bigint", nullable: false),
                    Base64Image = table.Column<string>(type: "text", nullable: false),
                    fileExtension = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    fileName = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_photo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_photo_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_photo_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_photo_memberModified",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_photo_receipt",
                        column: x => x.ReceiptId,
                        principalSchema: "receipt",
                        principalTable: "receipt",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_approval_MemberCreatedId",
                schema: "receipt",
                table: "approval",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_approval_MemberDeletedId",
                schema: "receipt",
                table: "approval",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_approval_MemberModifiedId",
                schema: "receipt",
                table: "approval",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_bill_MemberCreatedId",
                schema: "order",
                table: "bill",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_bill_MemberDeletedId",
                schema: "order",
                table: "bill",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_bill_MemberId",
                schema: "order",
                table: "bill",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_bill_MemberModifiedId",
                schema: "order",
                table: "bill",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_costCentre_MemberCreatedId",
                schema: "receipt",
                table: "costCentre",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_costCentre_MemberDeletedId",
                schema: "receipt",
                table: "costCentre",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_costCentre_MemberModifiedId",
                schema: "receipt",
                table: "costCentre",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_costCentre_Name",
                schema: "receipt",
                table: "costCentre",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_line_BillId",
                schema: "order",
                table: "line",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_line_MemberCreatedId",
                schema: "order",
                table: "line",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_line_MemberDeletedId",
                schema: "order",
                table: "line",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_line_MemberModifiedId",
                schema: "order",
                table: "line",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_line_ProductId",
                schema: "order",
                table: "line",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_member_MemberCreatedId",
                schema: "admin",
                table: "member",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_member_MemberDeletedId",
                schema: "admin",
                table: "member",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_member_MemberModifiedId",
                schema: "admin",
                table: "member",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_member_Name",
                schema: "admin",
                table: "member",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_memberPermission_MemberCreatedId",
                schema: "admin",
                table: "memberPermission",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_memberPermission_MemberDeletedId",
                schema: "admin",
                table: "memberPermission",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_memberPermission_MemberModifiedId",
                schema: "admin",
                table: "memberPermission",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_memberPermission_PermissionId",
                schema: "admin",
                table: "memberPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_MemberCreatedId",
                schema: "admin",
                table: "permission",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_MemberDeletedId",
                schema: "admin",
                table: "permission",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_MemberModifiedId",
                schema: "admin",
                table: "permission",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_Name",
                schema: "admin",
                table: "permission",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_photo_MemberCreatedId",
                schema: "receipt",
                table: "photo",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_photo_MemberDeletedId",
                schema: "receipt",
                table: "photo",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_photo_MemberModifiedId",
                schema: "receipt",
                table: "photo",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_photo_ReceiptId",
                schema: "receipt",
                table: "photo",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_product_MemberCreatedId",
                schema: "order",
                table: "product",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_product_MemberDeletedId",
                schema: "order",
                table: "product",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_product_MemberModifiedId",
                schema: "order",
                table: "product",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_productPrice_MemberCreatedId",
                schema: "order",
                table: "productPrice",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_productPrice_MemberDeletedId",
                schema: "order",
                table: "productPrice",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_productPrice_MemberModifiedId",
                schema: "order",
                table: "productPrice",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_CostCentreId",
                schema: "receipt",
                table: "receipt",
                column: "CostCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_MemberCreatedId",
                schema: "receipt",
                table: "receipt",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_MemberDeletedId",
                schema: "receipt",
                table: "receipt",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_MemberModifiedId",
                schema: "receipt",
                table: "receipt",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_MemberCreatedId",
                schema: "order",
                table: "stock",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_MemberDeletedId",
                schema: "order",
                table: "stock",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_MemberModifiedId",
                schema: "order",
                table: "stock",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_todo_MemberCreatedId",
                schema: "todo",
                table: "todo",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_todo_MemberDeletedId",
                schema: "todo",
                table: "todo",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_todo_MemberId",
                schema: "todo",
                table: "todo",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_todo_MemberModifiedId",
                schema: "todo",
                table: "todo",
                column: "MemberModifiedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "approval",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "approval",
                schema: "receiptHistory");

            migrationBuilder.DropTable(
                name: "bill",
                schema: "orderHistory");

            migrationBuilder.DropTable(
                name: "costCentre",
                schema: "receiptHistory");

            migrationBuilder.DropTable(
                name: "line",
                schema: "order");

            migrationBuilder.DropTable(
                name: "line",
                schema: "orderHistory");

            migrationBuilder.DropTable(
                name: "memberPermission",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "photo",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "photo",
                schema: "receiptHistory");

            migrationBuilder.DropTable(
                name: "product",
                schema: "orderHistory");

            migrationBuilder.DropTable(
                name: "productPrice",
                schema: "order");

            migrationBuilder.DropTable(
                name: "productPrice",
                schema: "orderHistory");

            migrationBuilder.DropTable(
                name: "receipt",
                schema: "receiptHistory");

            migrationBuilder.DropTable(
                name: "stock",
                schema: "order");

            migrationBuilder.DropTable(
                name: "stock",
                schema: "orderHistory");

            migrationBuilder.DropTable(
                name: "todo",
                schema: "todo");

            migrationBuilder.DropTable(
                name: "todo",
                schema: "todoHistory");

            migrationBuilder.DropTable(
                name: "bill",
                schema: "order");

            migrationBuilder.DropTable(
                name: "permission",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "receipt",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "product",
                schema: "order");

            migrationBuilder.DropTable(
                name: "costCentre",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "member",
                schema: "admin");

            migrationBuilder.DropSequence(
                name: "approvalSeq",
                schema: "receiptHistory");

            migrationBuilder.DropSequence(
                name: "billSeq",
                schema: "orderHistory");

            migrationBuilder.DropSequence(
                name: "costCentreSeq",
                schema: "receipt");

            migrationBuilder.DropSequence(
                name: "costCentreSeq",
                schema: "receiptHistory");

            migrationBuilder.DropSequence(
                name: "lineSeq",
                schema: "orderHistory");

            migrationBuilder.DropSequence(
                name: "memberSeq",
                schema: "admin");

            migrationBuilder.DropSequence(
                name: "permissionSeq",
                schema: "admin");

            migrationBuilder.DropSequence(
                name: "photoSeq",
                schema: "receipt");

            migrationBuilder.DropSequence(
                name: "photoSeq",
                schema: "receiptHistory");

            migrationBuilder.DropSequence(
                name: "productPriceSeq",
                schema: "orderHistory");

            migrationBuilder.DropSequence(
                name: "productSeq",
                schema: "order");

            migrationBuilder.DropSequence(
                name: "productSeq",
                schema: "orderHistory");

            migrationBuilder.DropSequence(
                name: "receiptSeq",
                schema: "receipt");

            migrationBuilder.DropSequence(
                name: "receiptSeq",
                schema: "receiptHistory");

            migrationBuilder.DropSequence(
                name: "stockSeq",
                schema: "orderHistory");

            migrationBuilder.DropSequence(
                name: "todoSeq",
                schema: "todo");

            migrationBuilder.DropSequence(
                name: "todoSeq",
                schema: "todoHistory");
        }
    }
}
