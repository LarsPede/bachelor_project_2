using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BachelorModelViewController.Migrations
{
    public partial class AddTokenUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", "'uuid-ossp', '', ''");

            migrationBuilder.AddColumn<Guid>(
                name: "Token",
                schema: "dbo",
                table: "Users",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", "'uuid-ossp', '', ''");
        }
    }
}
