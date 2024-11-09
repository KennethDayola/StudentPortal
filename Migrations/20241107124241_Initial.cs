using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Course = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Units = table.Column<int>(type: "int", nullable: false),
                    Offering = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Course = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Curriculum = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnrollmentHeaders",
                columns: table => new
                {
                    StudId = table.Column<int>(type: "int", nullable: false),
                    EnrollDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolYear = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Encoder = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TotalUnits = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentHeaders", x => x.StudId);
                    table.ForeignKey(
                        name: "FK_EnrollmentHeaders_Students_StudId",
                        column: x => x.StudId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prerequisites",
                columns: table => new
                {
                    SubjectCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PreCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prerequisites", x => new { x.SubjectCode, x.PreCode });
                    table.ForeignKey(
                        name: "FK_Prerequisites_Subjects_SubjectCode",
                        column: x => x.SubjectCode,
                        principalTable: "Subjects",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectSchedules",
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
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectSchedules", x => x.EDPCode);
                    table.ForeignKey(
                        name: "FK_SubjectSchedules_Subjects_SubjectCode",
                        column: x => x.SubjectCode,
                        principalTable: "Subjects",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnrollmentDetails",
                columns: table => new
                {
                    StudId = table.Column<int>(type: "int", nullable: false),
                    EDPCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    SubjectCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentDetails", x => new { x.StudId, x.EDPCode });
                    table.ForeignKey(
                        name: "FK_EnrollmentDetails_EnrollmentHeaders_StudId",
                        column: x => x.StudId,
                        principalTable: "EnrollmentHeaders",
                        principalColumn: "StudId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnrollmentDetails_SubjectSchedules_EDPCode",
                        column: x => x.EDPCode,
                        principalTable: "SubjectSchedules",
                        principalColumn: "EDPCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentDetails_EDPCode",
                table: "EnrollmentDetails",
                column: "EDPCode");

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_SubjectCode",
                table: "Prerequisites",
                column: "SubjectCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSchedules_SubjectCode",
                table: "SubjectSchedules",
                column: "SubjectCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnrollmentDetails");

            migrationBuilder.DropTable(
                name: "Prerequisites");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "EnrollmentHeaders");

            migrationBuilder.DropTable(
                name: "SubjectSchedules");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
