﻿// <auto-generated />
using System;
using Maasgroep.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Maasgroep.Database.Migrations
{
    [DbContext(typeof(MemberContext))]
    [Migration("20231203122959_000_memberContext")]
    partial class _000_memberContext
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("memberSeq", "admin");

            modelBuilder.HasSequence("permissionSeq", "admin");

            modelBuilder.Entity("Maasgroep.Database.Members.Member", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValueSql("nextval('admin.\"memberSeq\"')");

                    b.Property<DateTime>("DateTimeCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DateTimeDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberDeletedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberDeletedId");

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

                    b.Property<DateTime?>("DateTimeDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberDeletedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.HasKey("MemberId", "PermissionId");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberDeletedId");

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

                    b.Property<DateTime?>("DateTimeDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberDeletedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberDeletedId");

                    b.HasIndex("MemberModifiedId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("permission", "admin");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.CostCentre", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateTimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberDeletedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberDeletedId");

                    b.HasIndex("MemberModifiedId");

                    b.ToTable("CostCentre");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.Photo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Base64Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateTimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberDeletedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<long>("ReceiptId")
                        .HasColumnType("bigint");

                    b.Property<string>("fileExtension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("fileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberDeletedId");

                    b.HasIndex("MemberModifiedId");

                    b.HasIndex("ReceiptId");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.Receipt", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal?>("Amount")
                        .HasColumnType("numeric");

                    b.Property<long?>("CostCentreId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateTimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberDeletedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<string>("ReceiptStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CostCentreId");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberDeletedId");

                    b.HasIndex("MemberModifiedId");

                    b.ToTable("Receipt");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.ReceiptApproval", b =>
                {
                    b.Property<long>("ReceiptId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Approved")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("DateTimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("MemberCreatedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberDeletedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MemberModifiedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.HasKey("ReceiptId");

                    b.HasIndex("MemberCreatedId");

                    b.HasIndex("MemberDeletedId");

                    b.HasIndex("MemberModifiedId");

                    b.ToTable("ReceiptApproval");
                });

            modelBuilder.Entity("Maasgroep.Database.Members.Member", b =>
                {
                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("MembersCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_member_memberCreated");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberDeleted")
                        .WithMany("MembersDeleted")
                        .HasForeignKey("MemberDeletedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_member_memberDeleted");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("MembersModified")
                        .HasForeignKey("MemberModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_member_memberModified");

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberDeleted");

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

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberDeleted")
                        .WithMany("MemberPermissionsDeleted")
                        .HasForeignKey("MemberDeletedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_memberPermission_memberDeleted");

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

                    b.Navigation("MemberDeleted");

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

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberDeleted")
                        .WithMany("PermissionsDeleted")
                        .HasForeignKey("MemberDeletedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_permission_memberDeleted");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("PermissionsModified")
                        .HasForeignKey("MemberModifiedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_permission_memberModified");

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberDeleted");

                    b.Navigation("MemberModified");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.CostCentre", b =>
                {
                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("CostCentresCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberDeleted")
                        .WithMany("CostCentresDeleted")
                        .HasForeignKey("MemberDeletedId");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("CostCentresModified")
                        .HasForeignKey("MemberModifiedId");

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberDeleted");

                    b.Navigation("MemberModified");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.Photo", b =>
                {
                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("PhotosCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberDeleted")
                        .WithMany("PhotosDeleted")
                        .HasForeignKey("MemberDeletedId");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("PhotosModified")
                        .HasForeignKey("MemberModifiedId");

                    b.HasOne("Maasgroep.Database.Receipts.Receipt", "Receipt")
                        .WithMany("Photos")
                        .HasForeignKey("ReceiptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberDeleted");

                    b.Navigation("MemberModified");

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.Receipt", b =>
                {
                    b.HasOne("Maasgroep.Database.Receipts.CostCentre", "CostCentre")
                        .WithMany("Receipt")
                        .HasForeignKey("CostCentreId");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("ReceiptsCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberDeleted")
                        .WithMany("ReceiptsDeleted")
                        .HasForeignKey("MemberDeletedId");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("ReceiptsModified")
                        .HasForeignKey("MemberModifiedId");

                    b.Navigation("CostCentre");

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberDeleted");

                    b.Navigation("MemberModified");
                });

            modelBuilder.Entity("Maasgroep.Database.Receipts.ReceiptApproval", b =>
                {
                    b.HasOne("Maasgroep.Database.Members.Member", "MemberCreated")
                        .WithMany("ReceiptApprovalsCreated")
                        .HasForeignKey("MemberCreatedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberDeleted")
                        .WithMany("ReceiptApprovalsDeleted")
                        .HasForeignKey("MemberDeletedId");

                    b.HasOne("Maasgroep.Database.Members.Member", "MemberModified")
                        .WithMany("ReceiptApprovalsModified")
                        .HasForeignKey("MemberModifiedId");

                    b.HasOne("Maasgroep.Database.Receipts.Receipt", "Receipt")
                        .WithOne("ReceiptApproval")
                        .HasForeignKey("Maasgroep.Database.Receipts.ReceiptApproval", "ReceiptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MemberCreated");

                    b.Navigation("MemberDeleted");

                    b.Navigation("MemberModified");

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("Maasgroep.Database.Members.Member", b =>
                {
                    b.Navigation("CostCentresCreated");

                    b.Navigation("CostCentresDeleted");

                    b.Navigation("CostCentresModified");

                    b.Navigation("MemberPermissionsCreated");

                    b.Navigation("MemberPermissionsDeleted");

                    b.Navigation("MemberPermissionsModified");

                    b.Navigation("MembersCreated");

                    b.Navigation("MembersDeleted");

                    b.Navigation("MembersModified");

                    b.Navigation("Permissions");

                    b.Navigation("PermissionsCreated");

                    b.Navigation("PermissionsDeleted");

                    b.Navigation("PermissionsModified");

                    b.Navigation("PhotosCreated");

                    b.Navigation("PhotosDeleted");

                    b.Navigation("PhotosModified");

                    b.Navigation("ReceiptApprovalsCreated");

                    b.Navigation("ReceiptApprovalsDeleted");

                    b.Navigation("ReceiptApprovalsModified");

                    b.Navigation("ReceiptsCreated");

                    b.Navigation("ReceiptsDeleted");

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
                    b.Navigation("Photos");

                    b.Navigation("ReceiptApproval");
                });
#pragma warning restore 612, 618
        }
    }
}
