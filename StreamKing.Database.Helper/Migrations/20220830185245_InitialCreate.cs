using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StreamKing.Database.Helper.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FailedLogins = table.Column<int>(type: "integer", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    TmdbId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Tagline = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Release = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BackdropURL = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: false),
                    Runtime = table.Column<int>(type: "integer", nullable: true),
                    ImdbId = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<float>(type: "real", nullable: true),
                    VoteCount = table.Column<int>(type: "integer", nullable: true),
                    LastAirDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.TmdbId);
                });

            migrationBuilder.CreateTable(
                name: "AccountLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountLog_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    InstanceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    MediaTmdbId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.InstanceId);
                    table.ForeignKey(
                        name: "FK_Genre_Media_MediaTmdbId",
                        column: x => x.MediaTmdbId,
                        principalTable: "Media",
                        principalColumn: "TmdbId");
                });

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SeriesTmdbId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Season_Media_SeriesTmdbId",
                        column: x => x.SeriesTmdbId,
                        principalTable: "Media",
                        principalColumn: "TmdbId");
                });

            migrationBuilder.CreateTable(
                name: "StreamingInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    MediaTmdbId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamingInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamingInfo_Media_MediaTmdbId",
                        column: x => x.MediaTmdbId,
                        principalTable: "Media",
                        principalColumn: "TmdbId");
                });

            migrationBuilder.CreateTable(
                name: "Episode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    AirDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Rating = table.Column<double>(type: "double precision", nullable: true),
                    VoteCount = table.Column<int>(type: "integer", nullable: true),
                    StillPath = table.Column<string>(type: "text", nullable: true),
                    SeasonId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episode_Season_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Season",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountLog_AccountId",
                table: "AccountLog",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Episode_SeasonId",
                table: "Episode",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_MediaTmdbId",
                table: "Genre",
                column: "MediaTmdbId");

            migrationBuilder.CreateIndex(
                name: "IX_Season_SeriesTmdbId",
                table: "Season",
                column: "SeriesTmdbId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamingInfo_MediaTmdbId",
                table: "StreamingInfo",
                column: "MediaTmdbId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountLog");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "StreamingInfo");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "Media");
        }
    }
}
