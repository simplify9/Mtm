using Microsoft.EntityFrameworkCore.Migrations;

namespace SW.Mtm.MySql.Migrations
{
    public partial class Update6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileData",
                table: "Tenants",
                type: "json",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileData",
                table: "AccountTenantMemberships",
                type: "json",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Roles",
                table: "Accounts",
                unicode: false,
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldUnicode: false,
                oldMaxLength: 4000);

            migrationBuilder.AddColumn<string>(
                name: "ProfileData",
                table: "Accounts",
                type: "json",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "1",
                column: "Roles",
                value: "Mtm.Accounts.Create;Mtm.Accounts.ResetPassword;Mtm.Accounts.InitiatePasswordReset");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "2",
                column: "Roles",
                value: "Mtm.Accounts.Create");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "2d3d997abdaf4e2880f2b4737aab6b0d",
                column: "Roles",
                value: null);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "40ec4db42e434bf5a17f2065521a5219",
                column: "Roles",
                value: null);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "4a64f3640d914cfa98f3c166fe22f59a",
                column: "Roles",
                value: null);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "4cc3320b49af45dfb7ec13b072701acc",
                column: "Roles",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileData",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ProfileData",
                table: "AccountTenantMemberships");

            migrationBuilder.DropColumn(
                name: "ProfileData",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "Roles",
                table: "Accounts",
                type: "longtext CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "1",
                column: "Roles",
                value: "Mtm.Accounts.Login;Mtm.Accounts.Register;Mtm.Accounts.ResetPassword;Mtm.Accounts.InitiatePasswordReset");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "2",
                column: "Roles",
                value: "Mtm.Accounts.Register");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "2d3d997abdaf4e2880f2b4737aab6b0d",
                column: "Roles",
                value: "");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "40ec4db42e434bf5a17f2065521a5219",
                column: "Roles",
                value: "");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "4a64f3640d914cfa98f3c166fe22f59a",
                column: "Roles",
                value: "");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "4cc3320b49af45dfb7ec13b072701acc",
                column: "Roles",
                value: "");
        }
    }
}
