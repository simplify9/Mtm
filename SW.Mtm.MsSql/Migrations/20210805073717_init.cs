using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SW.Mtm.MsSql.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sequences",
                columns: table => new
                {
                    Entity = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sequences", x => x.Entity);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: false),
                    ProfileData = table.Column<string>(type: "json", nullable: true),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    Phone = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: false),
                    EmailProvider = table.Column<byte>(nullable: false),
                    LoginMethods = table.Column<byte>(nullable: false),
                    SecondFactorMethod = table.Column<byte>(nullable: false),
                    SecondFactorKey = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    IsSecondFactorKeyVerified = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    Landlord = table.Column<bool>(nullable: false),
                    EmailVerified = table.Column<bool>(nullable: false),
                    PhoneVerified = table.Column<bool>(nullable: false),
                    ProfileData = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Roles = table.Column<string>(unicode: false, maxLength: 4000, nullable: true),
                    Disabled = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountApiCredentials",
                columns: table => new
                {
                    AccountId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    Key = table.Column<string>(unicode: false, maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountApiCredentials", x => new { x.AccountId, x.Id });
                    table.ForeignKey(
                        name: "FK_AccountApiCredentials_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountTenantMemberships",
                columns: table => new
                {
                    AccountId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    ProfileData = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTenantMemberships", x => new { x.AccountId, x.Id });
                    table.ForeignKey(
                        name: "FK_AccountTenantMemberships_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountTenantMemberships_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    AccountId = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Email = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    Phone = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invitations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OtpTokens",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    AccountId = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Password = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    Type = table.Column<byte>(nullable: false),
                    LoginMethod = table.Column<byte>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtpTokens_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    AccountId = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetTokens_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    AccountId = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    LoginMethod = table.Column<byte>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Deleted", "DeletedBy", "DeletedOn", "Disabled", "DisplayName", "Email", "EmailProvider", "EmailVerified", "IsSecondFactorKeyVerified", "Landlord", "LoginMethods", "ModifiedBy", "ModifiedOn", "Password", "Phone", "PhoneVerified", "ProfileData", "Roles", "SecondFactorKey", "SecondFactorMethod", "TenantId" },
                values: new object[,]
                {
                    { "1", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "System", null, (byte)0, false, false, false, (byte)1, null, null, null, null, false, null, "Mtm.Accounts.Create;Mtm.Accounts.ResetPassword;Mtm.Accounts.InitiatePasswordReset", null, (byte)0, null },
                    { "2", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "Admin", "admin@xyz.com", (byte)0, true, false, true, (byte)2, null, null, "$SWHASH$V1$10000$VQCi48eitH4Ml5juvBMOFZrMdQwBbhuIQVXe6RR7qJdDF2bJ", null, true, null, "Mtm.Accounts.Create", null, (byte)0, null },
                    { "2d3d997abdaf4e2880f2b4737aab6b0d", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "Sample User", "sample@xyz.com", (byte)0, true, false, false, (byte)2, null, null, "$SWHASH$V1$10000$VQCi48eitH4Ml5juvBMOFZrMdQwBbhuIQVXe6RR7qJdDF2bJ", null, false, null, null, null, (byte)0, null },
                    { "4a64f3640d914cfa98f3c166fe22f59a", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "Sample User MFA", "samplewithmfa@xyz.com", (byte)0, true, false, false, (byte)2, null, null, "$SWHASH$V1$10000$VQCi48eitH4Ml5juvBMOFZrMdQwBbhuIQVXe6RR7qJdDF2bJ", null, false, null, null, null, (byte)1, null },
                    { "40ec4db42e434bf5a17f2065521a5219", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "Sample User Phone", null, (byte)0, false, false, false, (byte)4, null, null, null, "12345678", true, null, null, null, (byte)0, null },
                    { "4cc3320b49af45dfb7ec13b072701acc", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "Sample User API", null, (byte)0, false, false, false, (byte)1, null, null, null, null, false, null, null, null, (byte)0, null }
                });

            migrationBuilder.InsertData(
                table: "Sequences",
                columns: new[] { "Entity", "Value" },
                values: new object[] { "Tenant", 1 });

            migrationBuilder.InsertData(
                table: "AccountApiCredentials",
                columns: new[] { "AccountId", "Id", "Key", "Name" },
                values: new object[] { "1", 1, "dcc8edf250b04c94a31eb161fea11b5b", "default" });

            migrationBuilder.InsertData(
                table: "AccountApiCredentials",
                columns: new[] { "AccountId", "Id", "Key", "Name" },
                values: new object[] { "4cc3320b49af45dfb7ec13b072701acc", 2, "7facc758283844b49cc4ffd26a75b1de", "default" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountApiCredentials_Key",
                table: "AccountApiCredentials",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Phone",
                table: "Accounts",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_TenantId",
                table: "Accounts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTenantMemberships_TenantId",
                table: "AccountTenantMemberships",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_TenantId",
                table: "Invitations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_AccountId_TenantId",
                table: "Invitations",
                columns: new[] { "AccountId", "TenantId" },
                unique: true,
                filter: "[AccountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_Email_TenantId",
                table: "Invitations",
                columns: new[] { "Email", "TenantId" },
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_Phone_TenantId",
                table: "Invitations",
                columns: new[] { "Phone", "TenantId" },
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OtpTokens_AccountId",
                table: "OtpTokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_AccountId",
                table: "PasswordResetTokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AccountId",
                table: "RefreshTokens",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountApiCredentials");

            migrationBuilder.DropTable(
                name: "AccountTenantMemberships");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "OtpTokens");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Sequences");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
