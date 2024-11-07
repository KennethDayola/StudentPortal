using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class EnrollmentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnrollmentHeaderStudId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                        principalColumn: "Id");
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
                        principalColumn: "StudId");
                    table.ForeignKey(
                        name: "FK_EnrollmentDetails_SubjectSchedules_EDPCode",
                        column: x => x.EDPCode,
                        principalTable: "SubjectSchedules",
                        principalColumn: "EDPCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_EnrollmentHeaderStudId",
                table: "Students",
                column: "EnrollmentHeaderStudId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentDetails_EDPCode",
                table: "EnrollmentDetails",
                column: "EDPCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_EnrollmentHeaders_EnrollmentHeaderStudId",
                table: "Students",
                column: "EnrollmentHeaderStudId",
                principalTable: "EnrollmentHeaders",
                principalColumn: "StudId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_EnrollmentHeaders_EnrollmentHeaderStudId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "EnrollmentDetails");

            migrationBuilder.DropTable(
                name: "EnrollmentHeaders");

            migrationBuilder.DropIndex(
                name: "IX_Students_EnrollmentHeaderStudId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EnrollmentHeaderStudId",
                table: "Students");
        }
    }
}
