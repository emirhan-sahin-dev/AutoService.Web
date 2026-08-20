using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoService.DataAccess.Migrations
{
    public partial class AddSystemSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MileAge",
                table: "Vehicles",
                newName: "Mileage");

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    SystemSettingId = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation(
                            "SqlServer:Identity",
                            "1, 1"),

                    CompanyName = table.Column<string>(
                        type: "nvarchar(150)",
                        maxLength: 150,
                        nullable: false),

                    CompanyPhone = table.Column<string>(
                        type: "nvarchar(30)",
                        maxLength: 30,
                        nullable: true),

                    CompanyEmail = table.Column<string>(
                        type: "nvarchar(150)",
                        maxLength: 150,
                        nullable: true),

                    CompanyAddress = table.Column<string>(
                        type: "nvarchar(500)",
                        maxLength: 500,
                        nullable: true),

                    VatRate = table.Column<decimal>(
                        type: "decimal(5,2)",
                        nullable: false),

                    CriticalStockLevel = table.Column<int>(
                        type: "int",
                        nullable: false),

                    SessionTimeoutMinutes = table.Column<int>(
                        type: "int",
                        nullable: false),

                    Currency = table.Column<string>(
                        type: "nvarchar(10)",
                        maxLength: 10,
                        nullable: false),

                    CreatedDate = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false),

                    UpdatedDate = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: true),

                    DeletedDate = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: true),

                    IsDeleted = table.Column<bool>(
                        type: "bit",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_SystemSettings",
                        x => x.SystemSettingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.RenameColumn(
                name: "Mileage",
                table: "Vehicles",
                newName: "MileAge");
        }
    }
}