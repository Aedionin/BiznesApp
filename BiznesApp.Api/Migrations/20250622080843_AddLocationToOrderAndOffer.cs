using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiznesApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationToOrderAndOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Offers");
        }
    }
}
