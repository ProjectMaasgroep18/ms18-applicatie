﻿// <auto-generated />
using System;
using Maasgroep.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Maasgroep.Database.Migrations
{
    [DbContext(typeof(MaasgroepContext))]
    partial class MaasgroepContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Maasgroep.Database.Members.Member", b =>
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

                    b.Property<long?>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberModifiedId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("member", "admin");
                });

            modelBuilder.Entity("Maasgroep.Database.Members.MemberPermission", b =>
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

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.HasKey("MemberId", "PermissionId");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberModifiedId");

                    b.HasIndex("PermissionId");

                    b.ToTable("memberPermission", "admin");
                });

            modelBuilder.Entity("Maasgroep.Database.Members.Permission", b =>
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

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberModifiedId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("permission", "admin");
                });

            modelBuilder.Entity("Maasgroep.Database.Photos.Photo", b =>
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

            modelBuilder.Entity("Maasgroep.Database.Receipts.CostCentre", b =>
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

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberModifiedId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("costCentre", "receipt");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.Receipt", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValueSql("nextval('receipt.\"receiptSeq\"')");

                    b.Property<decimal?>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

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

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Note")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<long>("ReceiptStatusId")
                        .HasColumnType("bigint");

                    b.Property<long?>("StoreId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CostCentreId");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberModifiedId");

                    b.HasIndex("ReceiptStatusId");

                    b.ToTable("receipt", "receipt", t =>
                        {
                            t.HasCheckConstraint("CK_receipt_amount", "\"Amount\" >= 0");
                        });
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.ReceiptApproval", b =>
                {
                    b.Property<long>("ReceiptId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateTimeCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Note")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.HasKey("ReceiptId");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberModifiedId");

                    b.ToTable("approval", "receipt");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.ReceiptStatus", b =>
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

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberModifiedId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("status", "receipt");
                });

            modelBuilder.Entity("Maasgroep.Database.Members.Member", b =>
                {
                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("MembersCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_member_memberCreated");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("MembersModified")
                        .HasForeignKey("MemberModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_member_memberModified");

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberModified");
                });

            modelBuilder.Entity("Maasgroep.Database.Members.MemberPermission", b =>
                {
                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("MemberPermissionsCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_memberPermission_memberCreated");

                    b.HasOne("Maasgroep.Database.Members.Member", "Member")
                        .WithMany("Permissions")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_memberPermission_member");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("MemberPermissionsModified")
                        .HasForeignKey("MemberModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_memberPermission_memberModified");

                    b.HasOne("Maasgroep.Database.Members.Permission", "Permission")
                        .WithMany("Members")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_memberPermission_permission");

                    b.Navigation("Member");

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberModified");

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("Maasgroep.Database.Members.Permission", b =>
                {
                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("PermissionsCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_permission_memberCreated");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("PermissionsModified")
                        .HasForeignKey("MemberModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_permission_memberModified");

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberModified");
                });

            modelBuilder.Entity("Maasgroep.Database.Photos.Photo", b =>
                {
                    b.HasOne("Maasgroep.Database.Receipts.Receipt", "ReceiptInstance")
                        .WithOne("Photo")
                        .HasForeignKey("Maasgroep.Database.Photos.Photo", "Receipt")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_Photo_Receipt");

                    b.Navigation("ReceiptInstance");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.CostCentre", b =>
                {
                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("CostCentresCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_costCentre_memberCreated");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("CostCentresModified")
                        .HasForeignKey("MemberModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_costCentre_memberModified");

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberModified");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.Receipt", b =>
                {
                    b.HasOne("Maasgroep.Database.Receipts.CostCentre", "CostCentre")
                        .WithMany("Receipt")
                        .HasForeignKey("CostCentreId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_receipt_costCentre");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("ReceiptsCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_receipt_memberCreated");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("ReceiptsModified")
                        .HasForeignKey("MemberModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_receipt_memberModified");

                    b.HasOne("Maasgroep.Database.Receipts.ReceiptStatus", "ReceiptStatus")
                        .WithMany("Receipt")
                        .HasForeignKey("ReceiptStatusId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_receipt_receiptStatus");

                    b.Navigation("CostCentre");

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberModified");

                    b.Navigation("ReceiptStatus");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.ReceiptApproval", b =>
                {
                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("ReceiptApprovalsCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_receiptApproval_memberCreated");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("ReceiptApprovalsModified")
                        .HasForeignKey("MemberModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_receiptApproval_memberModified");

                    b.HasOne("Maasgroep.Database.Receipts.Receipt", "Receipt")
                        .WithOne("ReceiptApproval")
                        .HasForeignKey("Maasgroep.Database.Receipts.ReceiptApproval", "ReceiptId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_receiptApproval_receipt");

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberModified");

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.ReceiptStatus", b =>
                {
                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("ReceiptStatusesCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_receiptStatus_memberCreated");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("ReceiptStatusesModified")
                        .HasForeignKey("MemberModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_receiptStatus_memberModified");

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberModified");
                });

            modelBuilder.Entity("Maasgroep.Database.Members.Member", b =>
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
                });

            modelBuilder.Entity("Maasgroep.Database.Members.Permission", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.CostCentre", b =>
                {
                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.Receipt", b =>
                {
                    b.Navigation("Photo");

                    b.Navigation("ReceiptApproval");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.ReceiptStatus", b =>
                {
                    b.Navigation("Receipt");
                });
#pragma warning restore 612, 618
        }
    }
}
