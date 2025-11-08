using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_City.Migrations
{
    /// <inheritdoc />
    public partial class AutoMapperEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "UtilityIssues",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Suggestions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Complaints",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "UtilityIssues",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Suggestions",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Complaints",
                newName: "status");
        }
    }
}
