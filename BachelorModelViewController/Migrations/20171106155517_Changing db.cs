using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BachelorModelViewController.Migrations
{
    public partial class Changingdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Groups",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Associations",
                newSchema: "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Groups",
                schema: "dbo");

            migrationBuilder.RenameTable(
                name: "Associations",
                schema: "dbo");
        }
    }
}
