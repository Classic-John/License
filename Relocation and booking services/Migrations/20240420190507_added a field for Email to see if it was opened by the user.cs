using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Relocation_and_booking_services.Migrations
{
    /// <inheritdoc />
    public partial class addedafieldforEmailtoseeifitwasopenedbytheuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Opened",
                table: "Emails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Opened",
                table: "Emails");
        }
    }
}
