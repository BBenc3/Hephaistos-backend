using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHephaistos.Migrations
{
    /// <inheritdoc />
    public partial class subjectScheduleFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "classschedules_ibfk_1",
                table: "classschedules");

            migrationBuilder.AddForeignKey(
                name: "Subjectschedules_ibfk_1",
                table: "classschedules",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Subjectschedules_ibfk_1",
                table: "classschedules");

            migrationBuilder.AddForeignKey(
                name: "classschedules_ibfk_1",
                table: "classschedules",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
