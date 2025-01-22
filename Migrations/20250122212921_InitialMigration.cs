using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHephaistos.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int(11)", nullable: false),
                    Username = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8_hungarian_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Email = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8_hungarian_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8_hungarian_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    PasswordSalt = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8_hungarian_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    Role = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8_hungarian_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    active = table.Column<sbyte>(type: "tinyint(4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_hungarian_ci");

            migrationBuilder.CreateIndex(
                name: "Email",
                table: "users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
