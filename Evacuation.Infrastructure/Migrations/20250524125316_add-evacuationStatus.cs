using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evacuations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addevacuationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvacuationStatus_EvacuationZones_ZoneId",
                table: "EvacuationStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvacuationStatus",
                table: "EvacuationStatus");

            migrationBuilder.RenameTable(
                name: "EvacuationStatus",
                newName: "EvacuationStatuses");

            migrationBuilder.RenameIndex(
                name: "IX_EvacuationStatus_ZoneId",
                table: "EvacuationStatuses",
                newName: "IX_EvacuationStatuses_ZoneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvacuationStatuses",
                table: "EvacuationStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EvacuationStatuses_EvacuationZones_ZoneId",
                table: "EvacuationStatuses",
                column: "ZoneId",
                principalTable: "EvacuationZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvacuationStatuses_EvacuationZones_ZoneId",
                table: "EvacuationStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvacuationStatuses",
                table: "EvacuationStatuses");

            migrationBuilder.RenameTable(
                name: "EvacuationStatuses",
                newName: "EvacuationStatus");

            migrationBuilder.RenameIndex(
                name: "IX_EvacuationStatuses_ZoneId",
                table: "EvacuationStatus",
                newName: "IX_EvacuationStatus_ZoneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvacuationStatus",
                table: "EvacuationStatus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EvacuationStatus_EvacuationZones_ZoneId",
                table: "EvacuationStatus",
                column: "ZoneId",
                principalTable: "EvacuationZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
