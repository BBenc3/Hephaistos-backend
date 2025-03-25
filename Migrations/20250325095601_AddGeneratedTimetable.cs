using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHephaistos.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneratedTimetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsEvenSemester",
                table: "subjects",
                type: "bit",
                nullable: false,
                defaultValueSql: "'0'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "'0'");

            migrationBuilder.AlterColumn<bool>(
                name: "IsElective",
                table: "subjects",
                type: "bit",
                nullable: false,
                defaultValueSql: "'0'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "'0'");

            migrationBuilder.CreateTable(
                name: "SubjectPrerequisites",
                columns: table => new
                {
                    PrerequisiteId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectPrerequisites", x => new { x.PrerequisiteId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_SubjectPrerequisites_subjects_PrerequisiteId",
                        column: x => x.PrerequisiteId,
                        principalTable: "subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectPrerequisites_subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectPrerequisites_SubjectId",
                table: "SubjectPrerequisites",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectPrerequisites");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEvenSemester",
                table: "subjects",
                type: "bit",
                nullable: true,
                defaultValueSql: "'0'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "'0'");

            migrationBuilder.AlterColumn<bool>(
                name: "IsElective",
                table: "subjects",
                type: "bit",
                nullable: true,
                defaultValueSql: "'0'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "'0'");
        }
    }
}
