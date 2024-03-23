using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Relocation_and_booking_services.Migrations
{
    /// <inheritdoc />
    public partial class Columnfixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Furnitures");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Apartments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Transports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Schools",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Furnitures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
