using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SpareParts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SpareParts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Brands");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Vehicles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "SpareParts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ServiceRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ServiceDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Models",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Mechanics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Brands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "DeletedDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "DeletedDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "DeletedDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "DeletedDate",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "SpareParts");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Brands");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SpareParts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SpareParts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ServiceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ServiceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ServiceDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ServiceDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Models",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Models",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Mechanics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Mechanics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { null, null });
        }
    }
}
