using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceAcceptanceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdvisorName",
                table: "ServiceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerNotes",
                table: "ServiceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveredItems",
                table: "ServiceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExistingDamages",
                table: "ServiceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuelLevel",
                table: "ServiceRecords",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvisorName",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "CustomerNotes",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "DeliveredItems",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "ExistingDamages",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "FuelLevel",
                table: "ServiceRecords");
        }
    }
}
