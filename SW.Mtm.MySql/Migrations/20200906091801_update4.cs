using Microsoft.EntityFrameworkCore.Migrations;

namespace SW.Mtm.MySql.Migrations
{
    public partial class update4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "1",
                column: "Roles",
                value: "Mtm.Accounts.Login;Mtm.Accounts.Register");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "2",
                column: "Roles",
                value: "Mtm.Accounts.Register");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "1",
                column: "Roles",
                value: "Accounts.Login;Accounts.Register");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "2",
                column: "Roles",
                value: "Accounts.Register");
        }
    }
}
