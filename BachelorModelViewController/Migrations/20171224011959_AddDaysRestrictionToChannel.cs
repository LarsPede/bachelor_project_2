using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BachelorModelViewController.Migrations
{
    public partial class AddDaysRestrictionToChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_AccessRestrictions_AccessRestrictionId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "EndPoint",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "EntryPoint",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.AlterColumn<int>(
                name: "AccessRestrictionId",
                schema: "dbo",
                table: "Channels",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DaysRestriction",
                schema: "dbo",
                table: "Channels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_AccessRestrictions_AccessRestrictionId",
                schema: "dbo",
                table: "Channels",
                column: "AccessRestrictionId",
                principalSchema: "dbo",
                principalTable: "AccessRestrictions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_AccessRestrictions_AccessRestrictionId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "DaysRestriction",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.AlterColumn<int>(
                name: "AccessRestrictionId",
                schema: "dbo",
                table: "Channels",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "EndPoint",
                schema: "dbo",
                table: "Channels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntryPoint",
                schema: "dbo",
                table: "Channels",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_AccessRestrictions_AccessRestrictionId",
                schema: "dbo",
                table: "Channels",
                column: "AccessRestrictionId",
                principalSchema: "dbo",
                principalTable: "AccessRestrictions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
