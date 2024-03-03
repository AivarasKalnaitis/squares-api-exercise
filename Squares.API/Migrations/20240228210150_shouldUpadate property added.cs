using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Squares.API.Migrations
{
    /// <inheritdoc />
    public partial class shouldUpadatepropertyadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShouldUpdateSquares",
                table: "PointsLists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PointsLists_Points",
                columns: table => new
                {
                    PointsListId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsLists_Points", x => new { x.PointsListId, x.Id });
                    table.ForeignKey(
                        name: "FK_PointsLists_Points_PointsLists_PointsListId",
                        column: x => x.PointsListId,
                        principalTable: "PointsLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Square",
                columns: table => new
                {
                    PointsListId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Square", x => new { x.PointsListId, x.Id });
                    table.ForeignKey(
                        name: "FK_Square_PointsLists_PointsListId",
                        column: x => x.PointsListId,
                        principalTable: "PointsLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Square_Points",
                columns: table => new
                {
                    SquarePointsListId = table.Column<int>(type: "int", nullable: false),
                    SquareId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Square_Points", x => new { x.SquarePointsListId, x.SquareId, x.Id });
                    table.ForeignKey(
                        name: "FK_Square_Points_Square_SquarePointsListId_SquareId",
                        columns: x => new { x.SquarePointsListId, x.SquareId },
                        principalTable: "Square",
                        principalColumns: new[] { "PointsListId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointsLists_Points");

            migrationBuilder.DropTable(
                name: "Square_Points");

            migrationBuilder.DropTable(
                name: "Square");

            migrationBuilder.DropColumn(
                name: "ShouldUpdateSquares",
                table: "PointsLists");
        }
    }
}
