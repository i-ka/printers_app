using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication.Migrations
{
    public partial class field_rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cartridges_Offices_OfficeId",
                table: "Cartridges");

            migrationBuilder.DropIndex(
                name: "IX_Cartridges_OfficeId",
                table: "Cartridges");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "Cartridges");

            migrationBuilder.AddColumn<Guid>(
                name: "PlaceId",
                table: "Cartridges",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Cartridges_PlaceId",
                table: "Cartridges",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cartridges_Offices_PlaceId",
                table: "Cartridges",
                column: "PlaceId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cartridges_Offices_PlaceId",
                table: "Cartridges");

            migrationBuilder.DropIndex(
                name: "IX_Cartridges_PlaceId",
                table: "Cartridges");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Cartridges");

            migrationBuilder.AddColumn<Guid>(
                name: "OfficeId",
                table: "Cartridges",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Cartridges_OfficeId",
                table: "Cartridges",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cartridges_Offices_OfficeId",
                table: "Cartridges",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
