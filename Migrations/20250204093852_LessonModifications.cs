using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHephaistos.Migrations
{
    /// <inheritdoc />
    public partial class LessonModifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Lessons_PreviousLessonId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Lessons");

            migrationBuilder.AlterColumn<int>(
                name: "PreviousLessonId",
                table: "Lessons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Lessons_PreviousLessonId",
                table: "Lessons",
                column: "PreviousLessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Lessons_PreviousLessonId",
                table: "Lessons");

            migrationBuilder.AlterColumn<int>(
                name: "PreviousLessonId",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Lessons",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Lessons_PreviousLessonId",
                table: "Lessons",
                column: "PreviousLessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
