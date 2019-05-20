using Microsoft.EntityFrameworkCore.Migrations;

namespace Fiefdom.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fiefdom",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(nullable: true),
                    SessionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fiefdom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CurrentDay = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Market",
                columns: table => new
                {
                    Type = table.Column<string>(maxLength: 255, nullable: false),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Market", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "FiefdomPlot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Type = table.Column<string>(nullable: true),
                    FiefdomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiefdomPlot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FiefdomPlot_Fiefdom_FiefdomId",
                        column: x => x.FiefdomId,
                        principalTable: "Fiefdom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FiefdomResources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Type = table.Column<string>(nullable: true),
                    Quanity = table.Column<string>(nullable: true),
                    FiefdomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiefdomResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FiefdomResources_Fiefdom_FiefdomId",
                        column: x => x.FiefdomId,
                        principalTable: "Fiefdom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FiefdomPlot_FiefdomId",
                table: "FiefdomPlot",
                column: "FiefdomId");

            migrationBuilder.CreateIndex(
                name: "IX_FiefdomResources_FiefdomId",
                table: "FiefdomResources",
                column: "FiefdomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FiefdomPlot");

            migrationBuilder.DropTable(
                name: "FiefdomResources");

            migrationBuilder.DropTable(
                name: "GameState");

            migrationBuilder.DropTable(
                name: "Market");

            migrationBuilder.DropTable(
                name: "Fiefdom");
        }
    }
}
