using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SW.Mtm.Model;

namespace SW.Mtm.PgSql.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mtm");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "mtm",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "tenant",
                schema: "mtm",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    display_name = table.Column<string>(maxLength: 200, nullable: false),
                    profile_data = table.Column<IEnumerable<ProfileDataItem>>(type: "jsonb", nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    created_on = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    modified_on = table.Column<DateTime>(nullable: true),
                    deleted_by = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    deleted_on = table.Column<DateTime>(nullable: true),
                    deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tenant", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "account",
                schema: "mtm",
                columns: table => new
                {
                    id = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    email = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    phone = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    display_name = table.Column<string>(maxLength: 200, nullable: false),
                    email_provider = table.Column<byte>(nullable: false),
                    login_methods = table.Column<byte>(nullable: false),
                    second_factor_method = table.Column<byte>(nullable: false),
                    second_factor_key = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    password = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    landlord = table.Column<bool>(nullable: false),
                    email_verified = table.Column<bool>(nullable: false),
                    phone_verified = table.Column<bool>(nullable: false),
                    profile_data = table.Column<IEnumerable<ProfileDataItem>>(type: "jsonb", nullable: true),
                    tenant_id = table.Column<int>(nullable: true),
                    roles = table.Column<string[]>(maxLength: 100, nullable: true),
                    disabled = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    created_on = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    modified_on = table.Column<DateTime>(nullable: true),
                    deleted_by = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    deleted_on = table.Column<DateTime>(nullable: true),
                    deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalSchema: "mtm",
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "account_api_credential",
                schema: "mtm",
                columns: table => new
                {
                    account_id = table.Column<string>(nullable: false),
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 500, nullable: false),
                    key = table.Column<string>(unicode: false, maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_api_credential", x => new { x.account_id, x.id });
                    table.ForeignKey(
                        name: "fk_api_credential_account_account_id",
                        column: x => x.account_id,
                        principalSchema: "mtm",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_tenant_membership",
                schema: "mtm",
                columns: table => new
                {
                    account_id = table.Column<string>(nullable: false),
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tenant_id = table.Column<int>(nullable: false),
                    type = table.Column<byte>(nullable: false),
                    profile_data = table.Column<IEnumerable<ProfileDataItem>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tenant_membership", x => new { x.account_id, x.id });
                    table.ForeignKey(
                        name: "fk_tenant_membership_account_account_id",
                        column: x => x.account_id,
                        principalSchema: "mtm",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_account_tenant_membership_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalSchema: "mtm",
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invitation",
                schema: "mtm",
                columns: table => new
                {
                    id = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    account_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    email = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    phone = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    tenant_id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    created_on = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    modified_on = table.Column<DateTime>(nullable: true),
                    deleted_by = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    deleted_on = table.Column<DateTime>(nullable: true),
                    deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invitation", x => x.id);
                    table.ForeignKey(
                        name: "fk_invitation_account_account_id",
                        column: x => x.account_id,
                        principalSchema: "mtm",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invitation_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalSchema: "mtm",
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "otp_token",
                schema: "mtm",
                columns: table => new
                {
                    id = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    account_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    password = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    type = table.Column<byte>(nullable: false),
                    login_method = table.Column<byte>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_otp_token", x => x.id);
                    table.ForeignKey(
                        name: "fk_otp_token_account_account_id",
                        column: x => x.account_id,
                        principalSchema: "mtm",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "password_reset_token",
                schema: "mtm",
                columns: table => new
                {
                    id = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    account_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    created_on = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_password_reset_token", x => x.id);
                    table.ForeignKey(
                        name: "fk_password_reset_token_account_account_id",
                        column: x => x.account_id,
                        principalSchema: "mtm",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_token",
                schema: "mtm",
                columns: table => new
                {
                    id = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    account_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    login_method = table.Column<byte>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_token", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_token_account_account_id",
                        column: x => x.account_id,
                        principalSchema: "mtm",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "mtm",
                table: "account",
                columns: new[] { "id", "created_by", "created_on", "deleted", "deleted_by", "deleted_on", "disabled", "display_name", "email", "email_provider", "email_verified", "landlord", "login_methods", "modified_by", "modified_on", "password", "phone", "phone_verified", "profile_data", "roles", "second_factor_key", "second_factor_method", "tenant_id" },
                values: new object[,]
                {
                    { "1", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "System", null, (byte)0, false, false, (byte)1, null, null, null, null, false, null, new[] { "Mtm.Accounts.Create", "Mtm.Accounts.ResetPassword", "Mtm.Accounts.InitiatePasswordReset" }, null, (byte)0, null },
                    { "2", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "Admin", "admin@xyz.com", (byte)0, true, true, (byte)2, null, null, "$SWHASH$V1$10000$VQCi48eitH4Ml5juvBMOFZrMdQwBbhuIQVXe6RR7qJdDF2bJ", null, true, null, new[] { "Mtm.Accounts.Create" }, null, (byte)0, null },
                    { "2d3d997abdaf4e2880f2b4737aab6b0d", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "Sample User", "sample@xyz.com", (byte)0, true, false, (byte)2, null, null, "$SWHASH$V1$10000$VQCi48eitH4Ml5juvBMOFZrMdQwBbhuIQVXe6RR7qJdDF2bJ", null, false, null, null, null, (byte)0, null },
                    { "4a64f3640d914cfa98f3c166fe22f59a", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "Sample User MFA", "samplewithmfa@xyz.com", (byte)0, true, false, (byte)2, null, null, "$SWHASH$V1$10000$VQCi48eitH4Ml5juvBMOFZrMdQwBbhuIQVXe6RR7qJdDF2bJ", null, false, null, null, null, (byte)1, null },
                    { "40ec4db42e434bf5a17f2065521a5219", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "Sample User Phone", null, (byte)0, false, false, (byte)4, null, null, null, "12345678", true, null, null, null, (byte)0, null },
                    { "4cc3320b49af45dfb7ec13b072701acc", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, false, "Sample User API", null, (byte)0, false, false, (byte)1, null, null, null, null, false, null, null, null, (byte)0, null }
                });

            migrationBuilder.InsertData(
                schema: "mtm",
                table: "account_api_credential",
                columns: new[] { "account_id", "id", "key", "name" },
                values: new object[,]
                {
                    { "1", 1, "dcc8edf250b04c94a31eb161fea11b5b", "default" },
                    { "4cc3320b49af45dfb7ec13b072701acc", 2, "7facc758283844b49cc4ffd26a75b1de", "default" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_account_email",
                schema: "mtm",
                table: "account",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_account_phone",
                schema: "mtm",
                table: "account",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_account_tenant_id",
                schema: "mtm",
                table: "account",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_api_credential_key",
                schema: "mtm",
                table: "account_api_credential",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_account_tenant_membership_tenant_id",
                schema: "mtm",
                table: "account_tenant_membership",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_invitation_tenant_id",
                schema: "mtm",
                table: "invitation",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_invitation_account_id_tenant_id",
                schema: "mtm",
                table: "invitation",
                columns: new[] { "account_id", "tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invitation_email_tenant_id",
                schema: "mtm",
                table: "invitation",
                columns: new[] { "email", "tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invitation_phone_tenant_id",
                schema: "mtm",
                table: "invitation",
                columns: new[] { "phone", "tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_otp_token_account_id",
                schema: "mtm",
                table: "otp_token",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_password_reset_token_account_id",
                schema: "mtm",
                table: "password_reset_token",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_token_account_id",
                schema: "mtm",
                table: "refresh_token",
                column: "account_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_api_credential",
                schema: "mtm");

            migrationBuilder.DropTable(
                name: "account_tenant_membership",
                schema: "mtm");

            migrationBuilder.DropTable(
                name: "invitation",
                schema: "mtm");

            migrationBuilder.DropTable(
                name: "otp_token",
                schema: "mtm");

            migrationBuilder.DropTable(
                name: "password_reset_token",
                schema: "mtm");

            migrationBuilder.DropTable(
                name: "refresh_token",
                schema: "mtm");

            migrationBuilder.DropTable(
                name: "account",
                schema: "mtm");

            migrationBuilder.DropTable(
                name: "tenant",
                schema: "mtm");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "mtm");
        }
    }
}
