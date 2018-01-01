using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BachelorModelViewController.Migrations
{
    public partial class AddReqContentToChannels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_DatatypeModel_ContentId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.DropTable(
                name: "DatatypeModel");

            migrationBuilder.DropIndex(
                name: "IX_Channels_ContentId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "ContentId",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.AlterColumn<int>(
                name: "DaysRestriction",
                schema: "dbo",
                table: "Channels",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Content",
                schema: "dbo",
                table: "Channels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReqContent",
                schema: "dbo",
                table: "Channels",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "ReqContent",
                schema: "dbo",
                table: "Channels");

            migrationBuilder.AlterColumn<int>(
                name: "DaysRestriction",
                schema: "dbo",
                table: "Channels",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContentId",
                schema: "dbo",
                table: "Channels",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DatatypeModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatatypeModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Channels_ContentId",
                schema: "dbo",
                table: "Channels",
                column: "ContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_DatatypeModel_ContentId",
                schema: "dbo",
                table: "Channels",
                column: "ContentId",
                principalTable: "DatatypeModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
