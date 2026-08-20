using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceOperationTypeSparePart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceOperationTypeSpareParts",
                columns: table => new
                {
                    ServiceOperationTypeSparePartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceOperationTypeId = table.Column<int>(type: "int", nullable: false),
                    SparePartId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOperationTypeSpareParts", x => x.ServiceOperationTypeSparePartId);
                    table.ForeignKey(
                        name: "FK_ServiceOperationTypeSpareParts_ServiceOperationTypes_ServiceOperationTypeId",
                        column: x => x.ServiceOperationTypeId,
                        principalTable: "ServiceOperationTypes",
                        principalColumn: "ServiceOperationTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceOperationTypeSpareParts_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "SparePartId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOperationTypeSpareParts_ServiceOperationTypeId",
                table: "ServiceOperationTypeSpareParts",
                column: "ServiceOperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOperationTypeSpareParts_SparePartId",
                table: "ServiceOperationTypeSpareParts",
                column: "SparePartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceOperationTypeSpareParts");
        }
    }
}
