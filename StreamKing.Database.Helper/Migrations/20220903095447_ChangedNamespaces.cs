using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StreamKing.Database.Helper.Migrations
{
    public partial class ChangedNamespaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    Review = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRating", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Watchlist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchlist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Watchlist_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WatchEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tag = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserRatingId = table.Column<int>(type: "integer", nullable: true),
                    WatchlistId = table.Column<int>(type: "integer", nullable: false),
                    watchEntry_type = table.Column<string>(type: "text", nullable: false),
                    EpisodeId = table.Column<int>(type: "integer", nullable: true),
                    SeasonEntryId = table.Column<int>(type: "integer", nullable: true),
                    MovieTmdbId = table.Column<int>(type: "integer", nullable: true),
                    SeasonId = table.Column<int>(type: "integer", nullable: true),
                    SeriesEntryId = table.Column<int>(type: "integer", nullable: true),
                    SeriesTmdbId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchEntry_Episode_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchEntry_Media_MovieTmdbId",
                        column: x => x.MovieTmdbId,
                        principalTable: "Media",
                        principalColumn: "TmdbId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchEntry_Media_SeriesTmdbId",
                        column: x => x.SeriesTmdbId,
                        principalTable: "Media",
                        principalColumn: "TmdbId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchEntry_Season_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Season",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchEntry_UserRating_UserRatingId",
                        column: x => x.UserRatingId,
                        principalTable: "UserRating",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WatchEntry_WatchEntry_SeasonEntryId",
                        column: x => x.SeasonEntryId,
                        principalTable: "WatchEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchEntry_WatchEntry_SeriesEntryId",
                        column: x => x.SeriesEntryId,
                        principalTable: "WatchEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchEntry_Watchlist_WatchlistId",
                        column: x => x.WatchlistId,
                        principalTable: "Watchlist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchEntry_EpisodeId",
                table: "WatchEntry",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchEntry_MovieTmdbId",
                table: "WatchEntry",
                column: "MovieTmdbId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchEntry_SeasonEntryId",
                table: "WatchEntry",
                column: "SeasonEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchEntry_SeasonId",
                table: "WatchEntry",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchEntry_SeriesEntryId",
                table: "WatchEntry",
                column: "SeriesEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchEntry_SeriesTmdbId",
                table: "WatchEntry",
                column: "SeriesTmdbId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchEntry_UserRatingId",
                table: "WatchEntry",
                column: "UserRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchEntry_WatchlistId",
                table: "WatchEntry",
                column: "WatchlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Watchlist_AccountId",
                table: "Watchlist",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchEntry");

            migrationBuilder.DropTable(
                name: "UserRating");

            migrationBuilder.DropTable(
                name: "Watchlist");
        }
    }
}
