using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcertTicketsMarketWebApp.Migrations
{
    /// <inheritdoc />
    public partial class concert_model_flattened_location_removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concerts_Locations_PlaceId",
                table: "Concerts");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Concerts_PlaceId",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Concerts");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Concerts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Concerts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Concerts");

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "Concerts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Concerts_PlaceId",
                table: "Concerts",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Concerts_Locations_PlaceId",
                table: "Concerts",
                column: "PlaceId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
