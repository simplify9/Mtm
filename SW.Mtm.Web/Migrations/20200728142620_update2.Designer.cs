﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SW.Mtm.Api;

namespace SW.Mtm.Web.Migrations
{
    [DbContext(typeof(MtmDbContext))]
    [Migration("20200728142620_update2")]
    partial class update2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SW.EfCoreExtensions.Sequence", b =>
                {
                    b.Property<string>("Entity")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Entity");

                    b.ToTable("Sequences");

                    b.HasData(
                        new
                        {
                            Entity = "Tenant",
                            Value = 1
                        });
                });

            modelBuilder.Entity("SW.Mtm.Api.Domain.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .IsFixedLength(false)
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .IsFixedLength(false)
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Disabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<string>("Email")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200)
                        .IsUnicode(false);

                    b.Property<byte>("EmailProvider")
                        .HasColumnType("tinyint unsigned");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Landlord")
                        .HasColumnType("tinyint(1)");

                    b.Property<byte>("LoginMethods")
                        .HasColumnType("tinyint unsigned");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .IsFixedLength(false)
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Password")
                        .HasColumnType("varchar(500) CHARACTER SET utf8mb4")
                        .HasMaxLength(500)
                        .IsUnicode(false);

                    b.Property<string>("Phone")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<bool>("PhoneVerified")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasMaxLength(4000)
                        .IsUnicode(false);

                    b.Property<string>("SecondFactorKey")
                        .HasColumnType("varchar(500) CHARACTER SET utf8mb4")
                        .HasMaxLength(500)
                        .IsUnicode(false);

                    b.Property<byte>("SecondFactorMethod")
                        .HasColumnType("tinyint unsigned");

                    b.Property<int?>("TenantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.HasIndex("TenantId");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            CreatedOn = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deleted = false,
                            Disabled = false,
                            DisplayName = "System",
                            EmailProvider = (byte)0,
                            EmailVerified = false,
                            Landlord = false,
                            LoginMethods = (byte)1,
                            PhoneVerified = false,
                            Roles = "Accounts.Login;Accounts.Register",
                            SecondFactorMethod = (byte)0
                        },
                        new
                        {
                            Id = "2",
                            CreatedOn = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deleted = false,
                            Disabled = false,
                            DisplayName = "Admin",
                            Email = "admin@xyz.com",
                            EmailProvider = (byte)0,
                            EmailVerified = true,
                            Landlord = true,
                            LoginMethods = (byte)2,
                            Password = "$SWHASH$V1$10000$VQCi48eitH4Ml5juvBMOFZrMdQwBbhuIQVXe6RR7qJdDF2bJ",
                            PhoneVerified = true,
                            Roles = "Accounts.Register",
                            SecondFactorMethod = (byte)0
                        },
                        new
                        {
                            Id = "2d3d997abdaf4e2880f2b4737aab6b0d",
                            CreatedOn = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deleted = false,
                            Disabled = false,
                            DisplayName = "Sample User",
                            Email = "sample@xyz.com",
                            EmailProvider = (byte)0,
                            EmailVerified = true,
                            Landlord = false,
                            LoginMethods = (byte)2,
                            Password = "$SWHASH$V1$10000$VQCi48eitH4Ml5juvBMOFZrMdQwBbhuIQVXe6RR7qJdDF2bJ",
                            PhoneVerified = false,
                            Roles = "",
                            SecondFactorMethod = (byte)0
                        },
                        new
                        {
                            Id = "4a64f3640d914cfa98f3c166fe22f59a",
                            CreatedOn = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deleted = false,
                            Disabled = false,
                            DisplayName = "Sample User MFA",
                            Email = "samplewithmfa@xyz.com",
                            EmailProvider = (byte)0,
                            EmailVerified = true,
                            Landlord = false,
                            LoginMethods = (byte)2,
                            Password = "$SWHASH$V1$10000$VQCi48eitH4Ml5juvBMOFZrMdQwBbhuIQVXe6RR7qJdDF2bJ",
                            PhoneVerified = false,
                            Roles = "",
                            SecondFactorMethod = (byte)1
                        },
                        new
                        {
                            Id = "40ec4db42e434bf5a17f2065521a5219",
                            CreatedOn = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deleted = false,
                            Disabled = false,
                            DisplayName = "Sample User Phone",
                            EmailProvider = (byte)0,
                            EmailVerified = false,
                            Landlord = false,
                            LoginMethods = (byte)4,
                            Phone = "12345678",
                            PhoneVerified = true,
                            Roles = "",
                            SecondFactorMethod = (byte)0
                        },
                        new
                        {
                            Id = "4cc3320b49af45dfb7ec13b072701acc",
                            CreatedOn = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deleted = false,
                            Disabled = false,
                            DisplayName = "Sample User API",
                            EmailProvider = (byte)0,
                            EmailVerified = false,
                            Landlord = false,
                            LoginMethods = (byte)1,
                            PhoneVerified = false,
                            Roles = "",
                            SecondFactorMethod = (byte)0
                        });
                });

            modelBuilder.Entity("SW.Mtm.Api.Domain.Invitation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("AccountId")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .IsFixedLength(false)
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .IsFixedLength(false)
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200)
                        .IsUnicode(false);

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .IsFixedLength(false)
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Phone")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.HasIndex("AccountId", "TenantId")
                        .IsUnique();

                    b.HasIndex("Email", "TenantId")
                        .IsUnique();

                    b.HasIndex("Phone", "TenantId")
                        .IsUnique();

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("SW.Mtm.Api.Domain.OtpToken", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("AccountId")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<byte>("LoginMethod")
                        .HasColumnType("tinyint unsigned");

                    b.Property<string>("Password")
                        .HasColumnType("varchar(500) CHARACTER SET utf8mb4")
                        .HasMaxLength(500)
                        .IsUnicode(false);

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("OtpTokens");
                });

            modelBuilder.Entity("SW.Mtm.Api.Domain.RefreshToken", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("AccountId")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<byte>("LoginMethod")
                        .HasColumnType("tinyint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("SW.Mtm.Api.Domain.Tenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .IsFixedLength(false)
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .IsFixedLength(false)
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .IsFixedLength(false)
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("SW.Mtm.Api.Domain.Account", b =>
                {
                    b.HasOne("SW.Mtm.Api.Domain.Tenant", null)
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.OwnsMany("SW.Mtm.Api.Domain.ApiCredential", "ApiCredentials", b1 =>
                        {
                            b1.Property<string>("AccountId")
                                .HasColumnType("varchar(50) CHARACTER SET utf8mb4");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<string>("Key")
                                .IsRequired()
                                .HasColumnType("varchar(500) CHARACTER SET utf8mb4")
                                .HasMaxLength(500)
                                .IsUnicode(false);

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("varchar(500) CHARACTER SET utf8mb4")
                                .HasMaxLength(500);

                            b1.HasKey("AccountId", "Id");

                            b1.HasIndex("Key")
                                .IsUnique();

                            b1.ToTable("AccountApiCredentials");

                            b1.WithOwner()
                                .HasForeignKey("AccountId");

                            b1.HasData(
                                new
                                {
                                    AccountId = "1",
                                    Id = 1,
                                    Key = "dcc8edf250b04c94a31eb161fea11b5b",
                                    Name = "default"
                                },
                                new
                                {
                                    AccountId = "4cc3320b49af45dfb7ec13b072701acc",
                                    Id = 2,
                                    Key = "7facc758283844b49cc4ffd26a75b1de",
                                    Name = "default"
                                });
                        });

                    b.OwnsMany("SW.Mtm.Api.Domain.TenantMembership", "TenantMemberships", b1 =>
                        {
                            b1.Property<string>("AccountId")
                                .HasColumnType("varchar(50) CHARACTER SET utf8mb4");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<int>("TenantId")
                                .HasColumnType("int");

                            b1.Property<byte>("Type")
                                .HasColumnType("tinyint unsigned");

                            b1.HasKey("AccountId", "Id");

                            b1.HasIndex("TenantId");

                            b1.ToTable("AccountTenantMemberships");

                            b1.WithOwner()
                                .HasForeignKey("AccountId");

                            b1.HasOne("SW.Mtm.Api.Domain.Tenant", null)
                                .WithMany()
                                .HasForeignKey("TenantId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();
                        });
                });

            modelBuilder.Entity("SW.Mtm.Api.Domain.Invitation", b =>
                {
                    b.HasOne("SW.Mtm.Api.Domain.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SW.Mtm.Api.Domain.Tenant", null)
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SW.Mtm.Api.Domain.OtpToken", b =>
                {
                    b.HasOne("SW.Mtm.Api.Domain.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SW.Mtm.Api.Domain.RefreshToken", b =>
                {
                    b.HasOne("SW.Mtm.Api.Domain.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
