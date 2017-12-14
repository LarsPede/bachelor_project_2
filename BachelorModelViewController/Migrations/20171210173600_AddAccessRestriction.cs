using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BachelorModelViewController.Migrations
{
    public partial class AddAccessRestriction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Groups_GroupId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                schema: "dbo",
                table: "Channels",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "AccessRestrictionId",
                schema: "dbo",
                table: "Channels",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccessRestriction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AccessLevelId = table.Column<string>(nullable: true),
                    GroupRestricted = table.Column<bool>(nullable: false),
                    UserRestricted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessRestriction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessRestriction_Roles_AccessLevelId",
                        column: x => x.AccessLevelId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Channels_AccessRestrictionId",
                schema: "dbo",
                table: "Channels",
                column: "AccessRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRestriction_AccessLevelId",
                table: "AccessRestriction",
                column: "AccessLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_AccessRestriction_AccessRestrictionId",
                schema: "dbo",
                table: "Channels",
                column: "AccessRestrictionId",
                principalTable: "AccessRestriction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Groups_GroupId",
                schema: "dbo",
                table: "Channels",
                column: "GroupId",
                principalSchema: "dbo",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_AccessRestriction_AccessRestrictionId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Groups_GroupId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.DropTable(
                name: "AccessRestriction");

            migrationBuilder.DropIndex(
                name: "IX_Channels_AccessRestrictionId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "AccessRestrictionId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                schema: "dbo",
                table: "Channels",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Groups_GroupId",
                schema: "dbo",
                table: "Channels",
                column: "GroupId",
                principalSchema: "dbo",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
