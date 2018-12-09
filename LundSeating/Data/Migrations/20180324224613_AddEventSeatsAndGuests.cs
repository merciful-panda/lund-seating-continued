using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LundSeating.Data.Migrations
{
    public partial class AddEventSeatsAndGuests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sponsors",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Section",
                table: "Seats",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Row",
                table: "Seats",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Seats",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Guests",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Events",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Sponsors_Name",
                table: "Sponsors",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Seats_Section_Row_Number",
                table: "Seats",
                columns: new[] { "Section", "Row", "Number" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Guests_EmailAddress",
                table: "Guests",
                column: "EmailAddress");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Events_Name_Date",
                table: "Events",
                columns: new[] { "Name", "Date" });

            migrationBuilder.CreateTable(
                name: "EventGuests",
                columns: table => new
                {
                    EventGuestID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventID = table.Column<int>(nullable: false),
                    EventSeatID = table.Column<int>(nullable: true),
                    EventSponsorID = table.Column<int>(nullable: false),
                    GuestID = table.Column<int>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGuests", x => x.EventGuestID);
                    table.ForeignKey(
                        name: "FK_EventGuests_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventGuests_EventSponsors_EventSponsorID",
                        column: x => x.EventSponsorID,
                        principalTable: "EventSponsors",
                        principalColumn: "EventSponsorID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EventGuests_Guests_GuestID",
                        column: x => x.GuestID,
                        principalTable: "Guests",
                        principalColumn: "GuestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventSeats",
                columns: table => new
                {
                    EventSeatID = table.Column<int>(nullable: false),
                    EventGuestID = table.Column<int>(nullable: true),
                    EventID = table.Column<int>(nullable: false),
                    EventSponsorID = table.Column<int>(nullable: true),
                    SeatID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSeats", x => x.EventSeatID);
                    table.ForeignKey(
                        name: "FK_EventSeats_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSeats_EventGuests_EventSeatID",
                        column: x => x.EventSeatID,
                        principalTable: "EventGuests",
                        principalColumn: "EventGuestID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EventSeats_EventSponsors_EventSponsorID",
                        column: x => x.EventSponsorID,
                        principalTable: "EventSponsors",
                        principalColumn: "EventSponsorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventSeats_Seats_SeatID",
                        column: x => x.SeatID,
                        principalTable: "Seats",
                        principalColumn: "SeatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventGuests_EventID",
                table: "EventGuests",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventGuests_EventSponsorID",
                table: "EventGuests",
                column: "EventSponsorID");

            migrationBuilder.CreateIndex(
                name: "IX_EventGuests_GuestID",
                table: "EventGuests",
                column: "GuestID");

            migrationBuilder.CreateIndex(
                name: "IX_EventSeats_EventID",
                table: "EventSeats",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventSeats_EventSponsorID",
                table: "EventSeats",
                column: "EventSponsorID");

            migrationBuilder.CreateIndex(
                name: "IX_EventSeats_SeatID",
                table: "EventSeats",
                column: "SeatID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSeats");

            migrationBuilder.DropTable(
                name: "EventGuests");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Sponsors_Name",
                table: "Sponsors");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Seats_Section_Row_Number",
                table: "Seats");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Guests_EmailAddress",
                table: "Guests");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Events_Name_Date",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sponsors",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Section",
                table: "Seats",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Row",
                table: "Seats",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Seats",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Guests",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
