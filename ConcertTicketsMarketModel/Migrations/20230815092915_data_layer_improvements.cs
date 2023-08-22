using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcertTicketsMarketWebApp.Migrations
{
    /// <inheritdoc />
    public partial class data_layer_improvements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Performers_Performers_BandId",
                table: "Performers");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingTime",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Performers_Performers_BandId",
                table: "Performers",
                column: "BandId",
                principalTable: "Performers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Performers_Performers_BandId",
                table: "Performers");

            migrationBuilder.DropColumn(
                name: "BookingTime",
                table: "Tickets");

            migrationBuilder.AddForeignKey(
                name: "FK_Performers_Performers_BandId",
                table: "Performers",
                column: "BandId",
                principalTable: "Performers",
                principalColumn: "Id");
        }
    }
}
