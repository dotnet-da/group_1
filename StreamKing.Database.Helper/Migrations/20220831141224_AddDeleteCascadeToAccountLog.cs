using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamKing.Database.Helper.Migrations
{
    public partial class AddDeleteCascadeToAccountLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountLog_Accounts_AccountId",
                table: "AccountLog");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountLog_Accounts_AccountId",
                table: "AccountLog",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountLog_Accounts_AccountId",
                table: "AccountLog");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountLog_Accounts_AccountId",
                table: "AccountLog",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
