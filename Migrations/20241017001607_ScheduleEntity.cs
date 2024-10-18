using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Curriculum",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SubjectSchedule",
                columns: table => new
                {
                    EDPCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    SubjectCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Days = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Room = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    MaxSize = table.Column<int>(type: "int", nullable: false),
                    ClassSize = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    XM = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Section = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    SSFEDPCODE = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectSchedule", x => x.EDPCode);
                    table.ForeignKey(
                        name: "FK_SubjectSchedule_Subjects_SSFEDPCODE",
                        column: x => x.SSFEDPCODE,
                        principalTable: "Subjects",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSchedule_SSFEDPCODE",
                table: "SubjectSchedule",
                column: "SSFEDPCODE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectSchedule");

            migrationBuilder.AlterColumn<int>(
                name: "Curriculum",
                table: "Subjects",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Students",
                type: "nvarchar(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
