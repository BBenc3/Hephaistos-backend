using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHephaistos.Migrations
{
    /// <inheritdoc />
    public partial class removeMothresNameFromuserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "motherName",
                table: "Userdatas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "motherName",
                table: "Userdatas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
