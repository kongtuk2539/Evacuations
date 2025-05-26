using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evacuations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifyrequiredFKfalse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvacuationStatuses_EvacuationZones_EvacuationZoneId",
                table: "EvacuationStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_EvacuationStatuses_EvacuationZones_ZoneId",
                table: "EvacuationStatuses");

            migrationBuilder.DropIndex(
                name: "IX_EvacuationStatuses_EvacuationZoneId",
                table: "EvacuationStatuses");

            migrationBuilder.DropColumn(
                name: "EvacuationZoneId",
                table: "EvacuationStatuses");

            migrationBuilder.AddForeignKey(
                name: "FK_EvacuationStatuses_EvacuationZones_ZoneId",
                table: "EvacuationStatuses",
                column: "ZoneId",
                principalTable: "EvacuationZones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvacuationStatuses_EvacuationZones_ZoneId",
                table: "EvacuationStatuses");

            migrationBuilder.AddColumn<Guid>(
                name: "EvacuationZoneId",
                table: "EvacuationStatuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvacuationStatuses_EvacuationZoneId",
                table: "EvacuationStatuses",
                column: "EvacuationZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_EvacuationStatuses_EvacuationZones_EvacuationZoneId",
                table: "EvacuationStatuses",
                column: "EvacuationZoneId",
                principalTable: "EvacuationZones",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EvacuationStatuses_EvacuationZones_ZoneId",
                table: "EvacuationStatuses",
                column: "ZoneId",
                principalTable: "EvacuationZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
