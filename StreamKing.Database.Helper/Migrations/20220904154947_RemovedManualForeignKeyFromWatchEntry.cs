using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamKing.Database.Helper.Migrations
{
    public partial class RemovedManualForeignKeyFromWatchEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchEntry_Watchlist_WatchlistId",
                table: "WatchEntry");

            migrationBuilder.AlterColumn<int>(
                name: "WatchlistId",
                table: "WatchEntry",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "MovieEntry_WatchlistId",
                table: "WatchEntry",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WatchEntry_MovieEntry_WatchlistId",
                table: "WatchEntry",
                column: "MovieEntry_WatchlistId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchEntry_Watchlist_MovieEntry_WatchlistId",
                table: "WatchEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchEntry_Watchlist_WatchlistId",
                table: "WatchEntry");

            migrationBuilder.DropIndex(
                name: "IX_WatchEntry_MovieEntry_WatchlistId",
                table: "WatchEntry");

            migrationBuilder.DropColumn(
                name: "MovieEntry_WatchlistId",
                table: "WatchEntry");

            migrationBuilder.AlterColumn<int>(
                name: "WatchlistId",
                table: "WatchEntry",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchEntry_Watchlist_WatchlistId",
                table: "WatchEntry",
                column: "WatchlistId",
                principalTable: "Watchlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
