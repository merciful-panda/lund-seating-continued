using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LundSeating.Data.Migrations
{
    public partial class RemoveEmailAK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Guests_EmailAddress",
                table: "Guests");

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Guests",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Guests",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Guests_EmailAddress",
                table: "Guests",
                column: "EmailAddress");
        }
    }
}
