using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameVehicleFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VinNumnber",
                table: "Vehicles",
                newName: "VinNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_VinNumnber",
                table: "Vehicles",
                newName: "IX_Vehicles_VinNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VinNumber",
                table: "Vehicles",
                newName: "VinNumnber");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_VinNumber",
                table: "Vehicles",
                newName: "IX_Vehicles_VinNumnber");
        }
    }
}
