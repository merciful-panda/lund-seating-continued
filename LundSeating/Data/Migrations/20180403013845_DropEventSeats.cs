using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LundSeating.Data.Migrations
{
    public partial class DropEventSeats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSeats");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventSeats",
                columns: table => new
                {
                    EventSeatID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventGuestID = table.Column<int>(nullable: true),
                    EventID = table.Column<int>(nullable: false),
                    EventSponsorID = table.Column<int>(nullable: true),
                    SeatID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSeats", x => x.EventSeatID);
                    table.ForeignKey(
                        name: "FK_EventSeats_EventGuests_EventGuestID",
                        column: x => x.EventGuestID,
                        principalTable: "EventGuests",
                        principalColumn: "EventGuestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventSeats_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_EventSeats_EventGuestID",
                table: "EventSeats",
                column: "EventGuestID");

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
    }
}
