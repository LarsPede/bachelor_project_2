using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BachelorModelViewController.Migrations
{
    public partial class UpdateGroupConstrains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Groups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name",
                schema: "dbo",
                table: "Groups",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_Name",
                schema: "dbo",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "Groups");
        }
    }
}
