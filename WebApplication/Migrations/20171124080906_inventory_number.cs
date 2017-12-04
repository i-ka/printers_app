using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication.Migrations
{
    public partial class inventory_number : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InventoryNumber",
                table: "Cartridges",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cartridges_InventoryNumber",
                table: "Cartridges",
                column: "InventoryNumber",
                unique: true,
                filter: "[InventoryNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cartridges_InventoryNumber",
                table: "Cartridges");

            migrationBuilder.DropColumn(
                name: "InventoryNumber",
                table: "Cartridges");
        }
    }
}
