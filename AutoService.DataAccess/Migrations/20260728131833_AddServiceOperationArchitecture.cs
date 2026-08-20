using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceOperationArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDeliveryDate",
                table: "ServiceRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedDeliveryDate",
                table: "ServiceRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Mechanics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MechanicSpecialtyId",
                table: "Mechanics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MechanicSpecialties",
                columns: table => new
                {
                    MechanicSpecialtyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MechanicSpecialties", x => x.MechanicSpecialtyId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceOperationTypes",
                columns: table => new
                {
                    ServiceOperationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DefaultDurationHours = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    CustomerLaborPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MechanicPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MechanicSpecialtyId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOperationTypes", x => x.ServiceOperationTypeId);
                    table.ForeignKey(
                        name: "FK_ServiceOperationTypes_MechanicSpecialties_MechanicSpecialtyId",
                        column: x => x.MechanicSpecialtyId,
                        principalTable: "MechanicSpecialties",
                        principalColumn: "MechanicSpecialtyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceOperations",
                columns: table => new
                {
                    ServiceOperationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceRecordId = table.Column<int>(type: "int", nullable: false),
                    ServiceOperationTypeId = table.Column<int>(type: "int", nullable: false),
                    MechanicId = table.Column<int>(type: "int", nullable: false),
                    ProblemDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WorkDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LaborHours = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    CustomerLaborPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MechanicPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LaborGrossMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOperations", x => x.ServiceOperationId);
                    table.ForeignKey(
                        name: "FK_ServiceOperations_Mechanics_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Mechanics",
                        principalColumn: "MechanicId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceOperations_ServiceOperationTypes_ServiceOperationTypeId",
                        column: x => x.ServiceOperationTypeId,
                        principalTable: "ServiceOperationTypes",
                        principalColumn: "ServiceOperationTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceOperations_ServiceRecords_ServiceRecordId",
                        column: x => x.ServiceRecordId,
                        principalTable: "ServiceRecords",
                        principalColumn: "ServiceRecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceOperationParts",
                columns: table => new
                {
                    ServiceOperationPartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceOperationId = table.Column<int>(type: "int", nullable: false),
                    SparePartId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOperationParts", x => x.ServiceOperationPartId);
                    table.ForeignKey(
                        name: "FK_ServiceOperationParts_ServiceOperations_ServiceOperationId",
                        column: x => x.ServiceOperationId,
                        principalTable: "ServiceOperations",
                        principalColumn: "ServiceOperationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceOperationParts_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "SparePartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mechanics_MechanicSpecialtyId",
                table: "Mechanics",
                column: "MechanicSpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOperationParts_ServiceOperationId",
                table: "ServiceOperationParts",
                column: "ServiceOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOperationParts_SparePartId",
                table: "ServiceOperationParts",
                column: "SparePartId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOperations_MechanicId",
                table: "ServiceOperations",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOperations_ServiceOperationTypeId",
                table: "ServiceOperations",
                column: "ServiceOperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOperations_ServiceRecordId",
                table: "ServiceOperations",
                column: "ServiceRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOperationTypes_MechanicSpecialtyId",
                table: "ServiceOperationTypes",
                column: "MechanicSpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_MechanicSpecialties_MechanicSpecialtyId",
                table: "Mechanics",
                column: "MechanicSpecialtyId",
                principalTable: "MechanicSpecialties",
                principalColumn: "MechanicSpecialtyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_MechanicSpecialties_MechanicSpecialtyId",
                table: "Mechanics");

            migrationBuilder.DropTable(
                name: "ServiceOperationParts");

            migrationBuilder.DropTable(
                name: "ServiceOperations");

            migrationBuilder.DropTable(
                name: "ServiceOperationTypes");

            migrationBuilder.DropTable(
                name: "MechanicSpecialties");

            migrationBuilder.DropIndex(
                name: "IX_Mechanics_MechanicSpecialtyId",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "ActualDeliveryDate",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "EstimatedDeliveryDate",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "MechanicSpecialtyId",
                table: "Mechanics");
        }
    }
}
