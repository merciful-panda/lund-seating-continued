using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LundSeating.Data.Migrations
{
    public partial class UpdateEnums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SponsorshipLevel",
                table: "EventSponsors",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "EventGuests",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SponsorshipLevel",
                table: "EventSponsors",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "EventGuests",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
