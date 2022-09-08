using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamKing.Database.Helper.Migrations
{
    public partial class AddMoreCascadeDeletions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchEntry_Watchlist_MovieEntry_WatchlistId",
                table: "WatchEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchEntry_Watchlist_WatchlistId",
                table: "WatchEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_Watchlist_Accounts_AccountId",
                table: "Watchlist");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchEntry_Watchlist_MovieEntry_WatchlistId",
                table: "WatchEntry",
                column: "MovieEntry_WatchlistId",
                principalTable: "Watchlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchEntry_Watchlist_WatchlistId",
                table: "WatchEntry",
                column: "WatchlistId",
                principalTable: "Watchlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Watchlist_Accounts_AccountId",
                table: "Watchlist",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchEntry_Watchlist_MovieEntry_WatchlistId",
                table: "WatchEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchEntry_Watchlist_WatchlistId",
                table: "WatchEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_Watchlist_Accounts_AccountId",
                table: "Watchlist");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchEntry_Watchlist_MovieEntry_WatchlistId",
                table: "WatchEntry",
                column: "MovieEntry_WatchlistId",
                principalTable: "Watchlist",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchEntry_Watchlist_WatchlistId",
                table: "WatchEntry",
                column: "WatchlistId",
                principalTable: "Watchlist",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Watchlist_Accounts_AccountId",
                table: "Watchlist",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
