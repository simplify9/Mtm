using Microsoft.EntityFrameworkCore.Migrations;

namespace SW.Mtm.PgSql.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_second_factor_key_verified",
                schema: "mtm",
                table: "account",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_second_factor_key_verified",
                schema: "mtm",
                table: "account");
        }
    }
}
