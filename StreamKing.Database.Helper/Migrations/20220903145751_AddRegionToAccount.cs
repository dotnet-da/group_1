using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamKing.Database.Helper.Migrations
{
    public partial class AddRegionToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Accounts");
        }
    }
}
