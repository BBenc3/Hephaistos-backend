using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHephaistos.Migrations
{
    /// <inheritdoc />
    public partial class generatetimetablefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneratedTimetableId",
                table: "subjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_subjects_GeneratedTimetableId",
                table: "subjects",
                column: "GeneratedTimetableId");

            migrationBuilder.AddForeignKey(
                name: "FK_subjects_generatedtimetables_GeneratedTimetableId",
                table: "subjects",
                column: "GeneratedTimetableId",
                principalTable: "generatedtimetables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subjects_generatedtimetables_GeneratedTimetableId",
                table: "subjects");

            migrationBuilder.DropIndex(
                name: "IX_subjects_GeneratedTimetableId",
                table: "subjects");

            migrationBuilder.DropColumn(
                name: "GeneratedTimetableId",
                table: "subjects");
        }
    }
}
