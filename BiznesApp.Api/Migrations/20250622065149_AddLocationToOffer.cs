using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiznesApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationToOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Offers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Offers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Offers",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Offers");
        }
    }
}
