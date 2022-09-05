using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamKing.Database.Helper.Migrations
{
    public partial class AddedCascadeDeletionToSeasonAndSeriesEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchEntry_WatchEntry_SeasonEntryId",
                table: "WatchEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchEntry_WatchEntry_SeriesEntryId",
                table: "WatchEntry");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchEntry_WatchEntry_SeasonEntryId",
                table: "WatchEntry",
                column: "SeasonEntryId",
                principalTable: "WatchEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchEntry_WatchEntry_SeriesEntryId",
                table: "WatchEntry",
                column: "SeriesEntryId",
                principalTable: "WatchEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchEntry_WatchEntry_SeasonEntryId",
                table: "WatchEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchEntry_WatchEntry_SeriesEntryId",
                table: "WatchEntry");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchEntry_WatchEntry_SeasonEntryId",
                table: "WatchEntry",
                column: "SeasonEntryId",
                principalTable: "WatchEntry",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchEntry_WatchEntry_SeriesEntryId",
                table: "WatchEntry",
                column: "SeriesEntryId",
                principalTable: "WatchEntry",
                principalColumn: "Id");
        }
    }
}
