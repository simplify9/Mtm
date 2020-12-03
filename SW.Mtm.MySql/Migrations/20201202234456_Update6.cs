using Microsoft.EntityFrameworkCore.Migrations;

namespace SW.Mtm.MySql.Migrations
{
    public partial class Update6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileData",
                table: "AccountTenantMemberships",
                type: "json",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileData",
                table: "Accounts",
                type: "json",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileData",
                table: "AccountTenantMemberships");

            migrationBuilder.DropColumn(
                name: "ProfileData",
                table: "Accounts");
        }
    }
}
