﻿// <auto-generated />
using System;
using ms18_applicatie.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ms18_database.Migrations
{
    [DbContext(typeof(MaasgroepContext))]
    [Migration("20230930130205_kevin_001")]
    partial class kevin_001
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("costCentreSeq", "receipt");

            modelBuilder.HasSequence("memberSeq", "admin");

            modelBuilder.HasSequence("permissionSeq", "admin");

            modelBuilder.HasSequence("PhotoSeq", "photo");

            modelBuilder.HasSequence("receiptSeq", "receipt");

            modelBuilder.HasSequence("receiptStatusSeq", "receipt");

            modelBuilder.HasSequence("storeSeq", "receipt");

            modelBuilder.Entity("ms18_applicatie.Database.CostCentre", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValueSql("nextval('receipt.\"costCentreSeq\"')");

                    b.Property<DateTime>("DateTimeCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long>("UserCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserModifiedId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserCreatedId");

                    b.HasIndex("UserModifiedId");

                    b.ToTable("costCentre", "receipt");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Member", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValueSql("nextval('admin.\"memberSeq\"')");

                    b.Property<DateTime?>("DateTimeCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("UserCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserModifiedId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserCreatedId");

                    b.HasIndex("UserModifiedId");

                    b.ToTable("member", "admin");
                });

            modelBuilder.Entity("ms18_applicatie.Database.MemberPermission", b =>
                {
                    b.Property<long>("MemberId")
                        .HasColumnType("bigint");

                    b.Property<long>("PermissionId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateTimeCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserModifiedId")
                        .HasColumnType("bigint");

                    b.HasKey("MemberId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("UserCreatedId");

                    b.HasIndex("UserModifiedId");

                    b.ToTable("memberPermission", "admin");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Permission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValueSql("nextval('admin.\"permissionSeq\"')");

                    b.Property<DateTime>("DateTimeCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long>("UserCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserModifiedId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserCreatedId");

                    b.HasIndex("UserModifiedId");

                    b.ToTable("permission", "admin");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Photo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValueSql("nextval('photo.\"PhotoSeq\"')");

                    b.Property<byte[]>("Bytes")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<long?>("Receipt")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Receipt")
                        .IsUnique();

                    b.ToTable("photo", "photo");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Receipt", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValueSql("nextval('receipt.\"receiptSeq\"')");

                    b.Property<decimal?>("Amount")
                        .HasPrecision(2)
                        .HasColumnType("numeric(2)");

                    b.Property<long?>("CostCentreId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateTimeCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<long>("ReceiptStatusId")
                        .HasColumnType("bigint");

                    b.Property<long?>("StoreId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserModifiedId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CostCentreId");

                    b.HasIndex("ReceiptStatusId");

                    b.HasIndex("StoreId");

                    b.HasIndex("UserCreatedId");

                    b.HasIndex("UserModifiedId");

                    b.ToTable("receipt", "receipt");
                });

            modelBuilder.Entity("ms18_applicatie.Database.ReceiptApproval", b =>
                {
                    b.Property<long>("ReceiptId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateTimeCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Note")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<long>("UserCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserModifiedId")
                        .HasColumnType("bigint");

                    b.HasKey("ReceiptId");

                    b.HasIndex("UserCreatedId");

                    b.HasIndex("UserModifiedId");

                    b.ToTable("approval", "receipt");
                });

            modelBuilder.Entity("ms18_applicatie.Database.ReceiptStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValueSql("nextval('receipt.\"receiptStatusSeq\"')");

                    b.Property<DateTime>("DateTimeCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long>("UserCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserModifiedId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserCreatedId");

                    b.HasIndex("UserModifiedId");

                    b.ToTable("status", "receipt");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Store", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValueSql("nextval('receipt.\"storeSeq\"')");

                    b.Property<DateTime>("DateTimeCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long>("UserCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserModifiedId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserCreatedId");

                    b.HasIndex("UserModifiedId");

                    b.ToTable("store", "receipt");
                });

            modelBuilder.Entity("ms18_applicatie.Database.CostCentre", b =>
                {
                    b.HasOne("ms18_applicatie.Database.Member", "UserCreated")
                        .WithMany("CostCentresCreated")
                        .HasForeignKey("UserCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_costCentre_memberCreated");

                    b.HasOne("ms18_applicatie.Database.Member", "UserModified")
                        .WithMany("CostCentresModified")
                        .HasForeignKey("UserModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_costCentre_memberModified");

                    b.Navigation("UserCreated");

                    b.Navigation("UserModified");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Member", b =>
                {
                    b.HasOne("ms18_applicatie.Database.Member", "UserCreated")
                        .WithMany("MembersCreated")
                        .HasForeignKey("UserCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_member_memberCreated");

                    b.HasOne("ms18_applicatie.Database.Member", "UserModified")
                        .WithMany("MembersModified")
                        .HasForeignKey("UserModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_member_memberModified");

                    b.Navigation("UserCreated");

                    b.Navigation("UserModified");
                });

            modelBuilder.Entity("ms18_applicatie.Database.MemberPermission", b =>
                {
                    b.HasOne("ms18_applicatie.Database.Member", "Member")
                        .WithMany("Permissions")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_memberPermission_member");

                    b.HasOne("ms18_applicatie.Database.Permission", "Permission")
                        .WithMany("Members")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_memberPermission_permission");

                    b.HasOne("ms18_applicatie.Database.Member", "UserCreated")
                        .WithMany("MemberPermissionsCreated")
                        .HasForeignKey("UserCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_memberPermission_memberCreated");

                    b.HasOne("ms18_applicatie.Database.Member", "UserModified")
                        .WithMany("MemberPermissionsModified")
                        .HasForeignKey("UserModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_memberPermission_memberModified");

                    b.Navigation("Member");

                    b.Navigation("Permission");

                    b.Navigation("UserCreated");

                    b.Navigation("UserModified");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Permission", b =>
                {
                    b.HasOne("ms18_applicatie.Database.Member", "UserCreated")
                        .WithMany("PermissionsCreated")
                        .HasForeignKey("UserCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_permission_memberCreated");

                    b.HasOne("ms18_applicatie.Database.Member", "UserModified")
                        .WithMany("PermissionsModified")
                        .HasForeignKey("UserModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_permission_memberModified");

                    b.Navigation("UserCreated");

                    b.Navigation("UserModified");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Photo", b =>
                {
                    b.HasOne("ms18_applicatie.Database.Receipt", "ReceiptInstance")
                        .WithOne("Photo")
                        .HasForeignKey("ms18_applicatie.Database.Photo", "Receipt")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_Photo_Receipt");

                    b.Navigation("ReceiptInstance");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Receipt", b =>
                {
                    b.HasOne("ms18_applicatie.Database.CostCentre", "CostCentre")
                        .WithMany("Receipt")
                        .HasForeignKey("CostCentreId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_receipt_costCentre");

                    b.HasOne("ms18_applicatie.Database.ReceiptStatus", "ReceiptStatus")
                        .WithMany("Receipt")
                        .HasForeignKey("ReceiptStatusId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_receipt_receiptStatus");

                    b.HasOne("ms18_applicatie.Database.Store", "Store")
                        .WithMany("Receipt")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_receipt_store");

                    b.HasOne("ms18_applicatie.Database.Member", "UserCreated")
                        .WithMany("ReceiptsCreated")
                        .HasForeignKey("UserCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_receipt_memberCreated");

                    b.HasOne("ms18_applicatie.Database.Member", "UserModified")
                        .WithMany("ReceiptsModified")
                        .HasForeignKey("UserModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_receipt_memberModified");

                    b.Navigation("CostCentre");

                    b.Navigation("ReceiptStatus");

                    b.Navigation("Store");

                    b.Navigation("UserCreated");

                    b.Navigation("UserModified");
                });

            modelBuilder.Entity("ms18_applicatie.Database.ReceiptApproval", b =>
                {
                    b.HasOne("ms18_applicatie.Database.Receipt", "Receipt")
                        .WithOne("ReceiptApproval")
                        .HasForeignKey("ms18_applicatie.Database.ReceiptApproval", "ReceiptId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_receiptApproval_receipt");

                    b.HasOne("ms18_applicatie.Database.Member", "UserCreated")
                        .WithMany("ReceiptApprovalsCreated")
                        .HasForeignKey("UserCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_receiptApproval_memberCreated");

                    b.HasOne("ms18_applicatie.Database.Member", "UserModified")
                        .WithMany("ReceiptApprovalsModified")
                        .HasForeignKey("UserModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_receiptApproval_memberModified");

                    b.Navigation("Receipt");

                    b.Navigation("UserCreated");

                    b.Navigation("UserModified");
                });

            modelBuilder.Entity("ms18_applicatie.Database.ReceiptStatus", b =>
                {
                    b.HasOne("ms18_applicatie.Database.Member", "UserCreated")
                        .WithMany("ReceiptStatusesCreated")
                        .HasForeignKey("UserCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_receiptStatus_memberCreated");

                    b.HasOne("ms18_applicatie.Database.Member", "UserModified")
                        .WithMany("ReceiptStatusesModified")
                        .HasForeignKey("UserModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_receiptStatus_memberModified");

                    b.Navigation("UserCreated");

                    b.Navigation("UserModified");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Store", b =>
                {
                    b.HasOne("ms18_applicatie.Database.Member", "UserCreated")
                        .WithMany("StoresCreated")
                        .HasForeignKey("UserCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_store_memberCreated");

                    b.HasOne("ms18_applicatie.Database.Member", "UserModified")
                        .WithMany("StoresModified")
                        .HasForeignKey("UserModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_store_memberModified");

                    b.Navigation("UserCreated");

                    b.Navigation("UserModified");
                });

            modelBuilder.Entity("ms18_applicatie.Database.CostCentre", b =>
                {
                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Member", b =>
                {
                    b.Navigation("CostCentresCreated");

                    b.Navigation("CostCentresModified");

                    b.Navigation("MemberPermissionsCreated");

                    b.Navigation("MemberPermissionsModified");

                    b.Navigation("MembersCreated");

                    b.Navigation("MembersModified");

                    b.Navigation("Permissions");

                    b.Navigation("PermissionsCreated");

                    b.Navigation("PermissionsModified");

                    b.Navigation("ReceiptApprovalsCreated");

                    b.Navigation("ReceiptApprovalsModified");

                    b.Navigation("ReceiptStatusesCreated");

                    b.Navigation("ReceiptStatusesModified");

                    b.Navigation("ReceiptsCreated");

                    b.Navigation("ReceiptsModified");

                    b.Navigation("StoresCreated");

                    b.Navigation("StoresModified");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Permission", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Receipt", b =>
                {
                    b.Navigation("Photo");

                    b.Navigation("ReceiptApproval");
                });

            modelBuilder.Entity("ms18_applicatie.Database.ReceiptStatus", b =>
                {
                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("ms18_applicatie.Database.Store", b =>
                {
                    b.Navigation("Receipt");
                });
#pragma warning restore 612, 618
        }
    }
}
