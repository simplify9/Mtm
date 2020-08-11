using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SW.Mtm.Web.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Tenants_TenantId",
                table: "Invitations");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Invitations",
                unicode: false,
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200) CHARACTER SET utf8mb4",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Invitations",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Invitations",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Invitations",
                unicode: false,
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_AccountId_TenantId",
                table: "Invitations",
                columns: new[] { "AccountId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_Email_TenantId",
                table: "Invitations",
                columns: new[] { "Email", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_Phone_TenantId",
                table: "Invitations",
                columns: new[] { "Phone", "TenantId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Accounts_AccountId",
                table: "Invitations",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Tenants_TenantId",
                table: "Invitations",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Accounts_AccountId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Tenants_TenantId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_AccountId_TenantId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_Email_TenantId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_Phone_TenantId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Invitations");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Invitations",
                type: "varchar(200) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Invitations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Tenants_TenantId",
                table: "Invitations",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
