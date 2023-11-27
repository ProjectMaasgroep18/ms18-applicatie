using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ms18.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "receipt");

            migrationBuilder.EnsureSchema(
                name: "receiptHistory");

            migrationBuilder.EnsureSchema(
                name: "photoAlbum");

            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.EnsureSchema(
                name: "stock");

            migrationBuilder.EnsureSchema(
                name: "stockHistory");

            migrationBuilder.EnsureSchema(
                name: "photo");

            migrationBuilder.CreateSequence(
                name: "approvalSeq",
                schema: "receiptHistory");

            migrationBuilder.CreateSequence(
                name: "costCentreSeq",
                schema: "receipt");

            migrationBuilder.CreateSequence(
                name: "costCentreSeq",
                schema: "receiptHistory");

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
                name: "productSeq",
                schema: "stock");

            migrationBuilder.CreateSequence(
                name: "productSeq",
                schema: "stockHistory");

            migrationBuilder.CreateSequence(
                name: "receiptSeq",
                schema: "receipt");

            migrationBuilder.CreateSequence(
                name: "receiptSeq",
                schema: "receiptHistory");

            migrationBuilder.CreateSequence(
                name: "statusSeq",
                schema: "receipt");

            migrationBuilder.CreateSequence(
                name: "statusSeq",
                schema: "receiptHistory");

            migrationBuilder.CreateSequence(
                name: "stockSeq",
                schema: "stockHistory");

            migrationBuilder.CreateTable(
                name: "approval",
                schema: "receiptHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"receiptHistory\".\"approvalSeq\"')"),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    ReceiptId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
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
                name: "costCentre",
                schema: "receiptHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"receiptHistory\".\"costCentreSeq\"')"),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CostCentreId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
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
                name: "product",
                schema: "stockHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"stockHistory\".\"productSeq\"')"),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
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
                name: "receipt",
                schema: "receiptHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"receiptHistory\".\"receiptSeq\"')"),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    ReceiptId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    CostCentreId = table.Column<long>(type: "bigint", nullable: true),
                    ReceiptStatusId = table.Column<long>(type: "bigint", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
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
                name: "status",
                schema: "receiptHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"receiptHistory\".\"statusSeq\"')"),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    ReceiptStatusId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MemberCreatedId = table.Column<long>(type: "bigint", nullable: false),
                    MemberModifiedId = table.Column<long>(type: "bigint", nullable: true),
                    MemberDeletedId = table.Column<long>(type: "bigint", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stock",
                schema: "stockHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"stockHistory\".\"stockSeq\"')"),
                    RecordCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
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
                name: "costCentre",
                schema: "receipt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                name: "product",
                schema: "stock",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('stock.\"productSeq\"')"),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                        name: "FK_stockProduct_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_stockProduct_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_stockProduct_memberModified",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "status",
                schema: "receipt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('receipt.\"statusSeq\"')"),
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
                    table.PrimaryKey("PK_status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_receiptStatus_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receiptStatus_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receiptStatus_memberModified",
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

            migrationBuilder.CreateTable(
                name: "stock",
                schema: "stock",
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
                    table.CheckConstraint("CK_stock_quantity", "\"Quantity\" >= 0");
                    table.ForeignKey(
                        name: "FK_stock_memberCreated",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_stock_memberDeleted",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_stock_memberModified",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_stock_product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "stock",
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    ReceiptStatusId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_receipt_receiptStatus",
                        column: x => x.ReceiptStatusId,
                        principalSchema: "receipt",
                        principalTable: "status",
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
                name: "receiptPhotos",
                schema: "receipt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('photo.\"PhotoSeq\"')"),
                    Receipt = table.Column<long>(type: "bigint", nullable: true),
                    Bytes = table.Column<byte[]>(type: "bytea", nullable: true),
                    Base64Image = table.Column<string>(type: "text", nullable: false),
                    fileExtension = table.Column<string>(type: "text", nullable: false),
                    fileName = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_receiptPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photo_Receipt",
                        column: x => x.Receipt,
                        principalSchema: "receipt",
                        principalTable: "receipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_receiptPhotos_member_MemberCreatedId",
                        column: x => x.MemberCreatedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_receiptPhotos_member_MemberDeletedId",
                        column: x => x.MemberDeletedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_receiptPhotos_member_MemberModifiedId",
                        column: x => x.MemberModifiedId,
                        principalSchema: "admin",
                        principalTable: "member",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "member",
                columns: new[] { "Id", "DateTimeCreated", "DateTimeDeleted", "DateTimeModified", "MemberCreatedId", "MemberDeletedId", "MemberModifiedId", "Name" },
                values: new object[,]
                {
                    { 1L, new DateTime(2023, 11, 27, 16, 45, 56, 199, DateTimeKind.Utc).AddTicks(8540), null, null, 1L, null, 1L, "Borgia" },
                    { 2L, new DateTime(2023, 11, 27, 16, 45, 56, 199, DateTimeKind.Utc).AddTicks(8546), null, null, 1L, null, null, "da Gama" },
                    { 3L, new DateTime(2023, 11, 27, 16, 45, 56, 199, DateTimeKind.Utc).AddTicks(8547), null, null, 1L, null, null, "Albuquerque" }
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "permission",
                columns: new[] { "Id", "DateTimeDeleted", "DateTimeModified", "MemberCreatedId", "MemberDeletedId", "MemberModifiedId", "Name" },
                values: new object[,]
                {
                    { 1L, null, null, 1L, null, null, "receipt.approve" },
                    { 2L, null, null, 1L, null, null, "receipt.reject" },
                    { 3L, null, null, 1L, null, null, "receipt.handIn" },
                    { 4L, null, null, 1L, null, null, "receipt.payOut" }
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "memberPermission",
                columns: new[] { "MemberId", "PermissionId", "DateTimeDeleted", "DateTimeModified", "MemberCreatedId", "MemberDeletedId", "MemberModifiedId" },
                values: new object[,]
                {
                    { 2L, 1L, null, null, 1L, null, null },
                    { 2L, 2L, null, null, 1L, null, null },
                    { 3L, 3L, null, null, 1L, null, null }
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
                name: "IX_product_MemberCreatedId",
                schema: "stock",
                table: "product",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_product_MemberDeletedId",
                schema: "stock",
                table: "product",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_product_MemberModifiedId",
                schema: "stock",
                table: "product",
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
                name: "IX_receipt_ReceiptStatusId",
                schema: "receipt",
                table: "receipt",
                column: "ReceiptStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_receiptPhotos_MemberCreatedId",
                schema: "receipt",
                table: "receiptPhotos",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_receiptPhotos_MemberDeletedId",
                schema: "receipt",
                table: "receiptPhotos",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_receiptPhotos_MemberModifiedId",
                schema: "receipt",
                table: "receiptPhotos",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_receiptPhotos_Receipt",
                schema: "receipt",
                table: "receiptPhotos",
                column: "Receipt");

            migrationBuilder.CreateIndex(
                name: "IX_status_MemberCreatedId",
                schema: "receipt",
                table: "status",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_status_MemberDeletedId",
                schema: "receipt",
                table: "status",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_status_MemberModifiedId",
                schema: "receipt",
                table: "status",
                column: "MemberModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_status_Name",
                schema: "receipt",
                table: "status",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_stock_MemberCreatedId",
                schema: "stock",
                table: "stock",
                column: "MemberCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_MemberDeletedId",
                schema: "stock",
                table: "stock",
                column: "MemberDeletedId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_MemberModifiedId",
                schema: "stock",
                table: "stock",
                column: "MemberModifiedId");

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
                name: "approval",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "approval",
                schema: "receiptHistory");

            migrationBuilder.DropTable(
                name: "costCentre",
                schema: "receiptHistory");

            migrationBuilder.DropTable(
                name: "Likes",
                schema: "photoAlbum");

            migrationBuilder.DropTable(
                name: "memberPermission",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "PhotoTags",
                schema: "photoAlbum");

            migrationBuilder.DropTable(
                name: "product",
                schema: "stockHistory");

            migrationBuilder.DropTable(
                name: "receipt",
                schema: "receiptHistory");

            migrationBuilder.DropTable(
                name: "receiptPhotos",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "status",
                schema: "receiptHistory");

            migrationBuilder.DropTable(
                name: "stock",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "stock",
                schema: "stockHistory");

            migrationBuilder.DropTable(
                name: "permission",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "Photos",
                schema: "photoAlbum");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "photoAlbum");

            migrationBuilder.DropTable(
                name: "receipt",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "product",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "Folders",
                schema: "photoAlbum");

            migrationBuilder.DropTable(
                name: "costCentre",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "status",
                schema: "receipt");

            migrationBuilder.DropTable(
                name: "member",
                schema: "admin");

            migrationBuilder.DropSequence(
                name: "approvalSeq",
                schema: "receiptHistory");

            migrationBuilder.DropSequence(
                name: "costCentreSeq",
                schema: "receipt");

            migrationBuilder.DropSequence(
                name: "costCentreSeq",
                schema: "receiptHistory");

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
                name: "productSeq",
                schema: "stock");

            migrationBuilder.DropSequence(
                name: "productSeq",
                schema: "stockHistory");

            migrationBuilder.DropSequence(
                name: "receiptSeq",
                schema: "receipt");

            migrationBuilder.DropSequence(
                name: "receiptSeq",
                schema: "receiptHistory");

            migrationBuilder.DropSequence(
                name: "statusSeq",
                schema: "receipt");

            migrationBuilder.DropSequence(
                name: "statusSeq",
                schema: "receiptHistory");

            migrationBuilder.DropSequence(
                name: "stockSeq",
                schema: "stockHistory");
        }
    }
}
