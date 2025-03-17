using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHephaistos.Migrations
{
    /// <inheritdoc />
    public partial class azureinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "universities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Place = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<byte[]>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    Active = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "'1'"),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_University", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "majors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UniversityId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<byte[]>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    Active = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "'1'"),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Major", x => x.Id);
                    table.ForeignKey(
                        name: "majors_ibfk_1",
                        column: x => x.UniversityId,
                        principalTable: "universities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreditValue = table.Column<int>(type: "int", nullable: true),
                    MajorId = table.Column<int>(type: "int", nullable: true),
                    IsElective = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "'0'"),
                    IsEvenSemester = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "'0'"),
                    CreatedAt = table.Column<byte[]>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    Active = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "'1'"),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                    table.ForeignKey(
                        name: "subjects_ibfk_1",
                        column: x => x.MajorId,
                        principalTable: "majors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StartYear = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<byte[]>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    Active = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "'1'"),
                    Note = table.Column<string>(type: "text", nullable: true),
                    MajorId = table.Column<int>(type: "int", nullable: true),
                    ProfilePicturepath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "fk_users_major",
                        column: x => x.MajorId,
                        principalTable: "majors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "classschedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    DayOfWeek = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    CreatedAt = table.Column<byte[]>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    active = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classschedule", x => x.Id);
                    table.ForeignKey(
                        name: "classschedules_ibfk_1",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "auditlog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: true),
                    OperationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChangedData = table.Column<string>(type: "json", nullable: true),
                    ChangedAt = table.Column<byte[]>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    ChangedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditlog", x => x.Id);
                    table.ForeignKey(
                        name: "auditlog_ibfk_1",
                        column: x => x.ChangedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "completedsubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Completedsubject", x => x.Id);
                    table.ForeignKey(
                        name: "completedsubjects_ibfk_1",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "completedsubjects_ibfk_2",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "refreshtokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", maxLength: 6, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", maxLength: 6, nullable: true),
                    Revoked = table.Column<DateTime>(type: "datetime2", maxLength: 6, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<byte[]>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    Active = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refreshtoken", x => x.Id);
                    table.ForeignKey(
                        name: "refreshtokens_ibfk_1",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "ChangedBy",
                table: "auditlog",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "SubjectId",
                table: "classschedules",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "SubjectId",
                table: "completedsubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "UserId",
                table: "completedsubjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UniversityId",
                table: "majors",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "UserId",
                table: "refreshtokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "MajorId",
                table: "subjects",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "fk_users_major",
                table: "users",
                column: "MajorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auditlog");

            migrationBuilder.DropTable(
                name: "classschedules");

            migrationBuilder.DropTable(
                name: "completedsubjects");

            migrationBuilder.DropTable(
                name: "course");

            migrationBuilder.DropTable(
                name: "refreshtokens");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "majors");

            migrationBuilder.DropTable(
                name: "universities");
        }
    }
}
