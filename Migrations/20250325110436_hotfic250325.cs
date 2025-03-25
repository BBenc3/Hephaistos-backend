using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHephaistos.Migrations
{
    /// <inheritdoc />
    public partial class hotfic250325 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneratedTimetableId",
                table: "classschedules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "generatedtimetables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<byte[]>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    Active = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedTimetable", x => x.Id);
                    table.ForeignKey(
                        name: "generatedtimetables_ibfk_1",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_classschedules_GeneratedTimetableId",
                table: "classschedules",
                column: "GeneratedTimetableId");

            migrationBuilder.CreateIndex(
                name: "UserId",
                table: "generatedtimetables",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_classschedules_generatedtimetables_GeneratedTimetableId",
                table: "classschedules",
                column: "GeneratedTimetableId",
                principalTable: "generatedtimetables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_classschedules_generatedtimetables_GeneratedTimetableId",
                table: "classschedules");

            migrationBuilder.DropTable(
                name: "generatedtimetables");

            migrationBuilder.DropIndex(
                name: "IX_classschedules_GeneratedTimetableId",
                table: "classschedules");

            migrationBuilder.DropColumn(
                name: "GeneratedTimetableId",
                table: "classschedules");
        }
    }
}
