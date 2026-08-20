using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMechanicFromServiceRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceDetails_ServiceRecords_ServiceRecordId",
                table: "ServiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOperations_ServiceRecords_ServiceRecordId",
                table: "ServiceOperations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRecords_Mechanics_MechanicId",
                table: "ServiceRecords");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRecords_MechanicId",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "MechanicId",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "MechanicNote",
                table: "ServiceRecords");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EstimatedDeliveryDate",
                table: "ServiceRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ServiceRecords",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceDetails_ServiceRecords_ServiceRecordId",
                table: "ServiceDetails",
                column: "ServiceRecordId",
                principalTable: "ServiceRecords",
                principalColumn: "ServiceRecordId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOperations_ServiceRecords_ServiceRecordId",
                table: "ServiceOperations",
                column: "ServiceRecordId",
                principalTable: "ServiceRecords",
                principalColumn: "ServiceRecordId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceDetails_ServiceRecords_ServiceRecordId",
                table: "ServiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOperations_ServiceRecords_ServiceRecordId",
                table: "ServiceOperations");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EstimatedDeliveryDate",
                table: "ServiceRecords",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ServiceRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "ServiceRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MechanicId",
                table: "ServiceRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MechanicNote",
                table: "ServiceRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRecords_MechanicId",
                table: "ServiceRecords",
                column: "MechanicId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceDetails_ServiceRecords_ServiceRecordId",
                table: "ServiceDetails",
                column: "ServiceRecordId",
                principalTable: "ServiceRecords",
                principalColumn: "ServiceRecordId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOperations_ServiceRecords_ServiceRecordId",
                table: "ServiceOperations",
                column: "ServiceRecordId",
                principalTable: "ServiceRecords",
                principalColumn: "ServiceRecordId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRecords_Mechanics_MechanicId",
                table: "ServiceRecords",
                column: "MechanicId",
                principalTable: "Mechanics",
                principalColumn: "MechanicId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
