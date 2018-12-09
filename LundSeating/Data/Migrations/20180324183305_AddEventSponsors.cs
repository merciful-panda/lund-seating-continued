using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LundSeating.Data.Migrations
{
    public partial class AddEventSponsors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventSponsors",
                columns: table => new
                {
                    EventSponsorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventID = table.Column<int>(nullable: false),
                    HostID = table.Column<int>(nullable: true),
                    NumTicketsAllocated = table.Column<int>(nullable: true),
                    SponsorID = table.Column<int>(nullable: false),
                    SponsorshipLevel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSponsors", x => x.EventSponsorID);
                    table.ForeignKey(
                        name: "FK_EventSponsors_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSponsors_Guests_HostID",
                        column: x => x.HostID,
                        principalTable: "Guests",
                        principalColumn: "GuestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventSponsors_Sponsors_SponsorID",
                        column: x => x.SponsorID,
                        principalTable: "Sponsors",
                        principalColumn: "SponsorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventSponsors_EventID",
                table: "EventSponsors",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventSponsors_HostID",
                table: "EventSponsors",
                column: "HostID");

            migrationBuilder.CreateIndex(
                name: "IX_EventSponsors_SponsorID",
                table: "EventSponsors",
                column: "SponsorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSponsors");
        }
    }
}
