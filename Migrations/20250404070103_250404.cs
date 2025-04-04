using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHephaistos.Migrations
{
    /// <inheritdoc />
    public partial class _250404 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StartYear",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicturepath",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "users",
                type: "bit",
                nullable: false,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "universities",
                type: "bit",
                nullable: false,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "subjects",
                type: "bit",
                nullable: false,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "refreshtokens",
                type: "datetime2",
                maxLength: 6,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "refreshtokens",
                type: "bit",
                nullable: false,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "majors",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "majors",
                type: "bit",
                nullable: false,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "generatedtimetables",
                type: "bit",
                nullable: false,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "active",
                table: "completedsubjects",
                type: "bit",
                nullable: false,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "active",
                table: "classschedules",
                type: "bit",
                nullable: false,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "'1'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "StartYear",
                table: "users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicturepath",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "users",
                type: "bit",
                nullable: true,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "universities",
                type: "bit",
                nullable: true,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "subjects",
                type: "bit",
                nullable: true,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "refreshtokens",
                type: "datetime2",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "refreshtokens",
                type: "bit",
                nullable: true,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "majors",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "majors",
                type: "bit",
                nullable: true,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "generatedtimetables",
                type: "bit",
                nullable: true,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "active",
                table: "completedsubjects",
                type: "bit",
                nullable: true,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "active",
                table: "classschedules",
                type: "bit",
                nullable: true,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "'1'");
        }
    }
}
