using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication.Migrations
{
    public partial class nullable_printerid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cartridges_InventoryNumber",
                table: "Cartridges");

            migrationBuilder.AlterColumn<Guid>(
                name: "CartridgeId",
                table: "Printers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "InventoryNumber",
                table: "Cartridges",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cartridges_InventoryNumber",
                table: "Cartridges",
                column: "InventoryNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cartridges_InventoryNumber",
                table: "Cartridges");

            migrationBuilder.AlterColumn<Guid>(
                name: "CartridgeId",
                table: "Printers",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InventoryNumber",
                table: "Cartridges",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.CreateIndex(
                name: "IX_Cartridges_InventoryNumber",
                table: "Cartridges",
                column: "InventoryNumber",
                unique: true,
                filter: "[InventoryNumber] IS NOT NULL");
        }
    }
}
