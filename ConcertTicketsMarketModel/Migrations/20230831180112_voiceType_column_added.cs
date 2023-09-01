using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcertTicketsMarketWebApp.Migrations
{
    /// <inheritdoc />
    public partial class voiceType_column_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoiceType",
                table: "Performers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoiceType",
                table: "Performers");
        }
    }
}
