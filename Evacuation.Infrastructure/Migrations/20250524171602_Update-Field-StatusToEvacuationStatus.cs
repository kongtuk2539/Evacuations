using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evacuations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldStatusToEvacuationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EvacuationStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "EvacuationStatuses");
        }
    }
}
