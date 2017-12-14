using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BachelorModelViewController.Migrations
{
    public partial class AccessRestrictionToDbo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessRestriction_Roles_AccessLevelId",
                table: "AccessRestriction");

            migrationBuilder.DropForeignKey(
                name: "FK_Channels_AccessRestriction_AccessRestrictionId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessRestriction",
                table: "AccessRestriction");

            migrationBuilder.RenameTable(
                name: "AccessRestriction",
                newName: "AccessRestrictions",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_AccessRestriction_AccessLevelId",
                schema: "dbo",
                table: "AccessRestrictions",
                newName: "IX_AccessRestrictions_AccessLevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessRestrictions",
                schema: "dbo",
                table: "AccessRestrictions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessRestrictions_Roles_AccessLevelId",
                schema: "dbo",
                table: "AccessRestrictions",
                column: "AccessLevelId",
                principalSchema: "dbo",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessRestrictions_Roles_AccessLevelId",
                schema: "dbo",
                table: "AccessRestrictions");

            migrationBuilder.DropForeignKey(
                name: "FK_Channels_AccessRestrictions_AccessRestrictionId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessRestrictions",
                schema: "dbo",
                table: "AccessRestrictions");

            migrationBuilder.RenameTable(
                name: "AccessRestrictions",
                schema: "dbo",
                newName: "AccessRestriction");

            migrationBuilder.RenameIndex(
                name: "IX_AccessRestrictions_AccessLevelId",
                table: "AccessRestriction",
                newName: "IX_AccessRestriction_AccessLevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessRestriction",
                table: "AccessRestriction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessRestriction_Roles_AccessLevelId",
                table: "AccessRestriction",
                column: "AccessLevelId",
                principalSchema: "dbo",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_AccessRestriction_AccessRestrictionId",
                schema: "dbo",
                table: "Channels",
                column: "AccessRestrictionId",
                principalTable: "AccessRestriction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
