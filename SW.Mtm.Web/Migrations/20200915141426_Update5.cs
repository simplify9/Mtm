using Microsoft.EntityFrameworkCore.Migrations;

namespace SW.Mtm.Web.Migrations
{
    public partial class Update5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "1",
                column: "Roles",
                value: "Mtm.Accounts.Login;Mtm.Accounts.Register;Mtm.Accounts.ResetPassword");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "1",
                column: "Roles",
                value: "Mtm.Accounts.Login;Mtm.Accounts.Register");
        }
    }
}
