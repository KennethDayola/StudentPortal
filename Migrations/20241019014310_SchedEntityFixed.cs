using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class SchedEntityFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSchedule_Subjects_SubjectCode",
                table: "SubjectSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectSchedule",
                table: "SubjectSchedule");

            migrationBuilder.RenameTable(
                name: "SubjectSchedule",
                newName: "SubjectSchedules");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectSchedule_SubjectCode",
                table: "SubjectSchedules",
                newName: "IX_SubjectSchedules_SubjectCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectSchedules",
                table: "SubjectSchedules",
                column: "EDPCode");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSchedules_Subjects_SubjectCode",
                table: "SubjectSchedules",
                column: "SubjectCode",
                principalTable: "Subjects",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSchedules_Subjects_SubjectCode",
                table: "SubjectSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectSchedules",
                table: "SubjectSchedules");

            migrationBuilder.RenameTable(
                name: "SubjectSchedules",
                newName: "SubjectSchedule");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectSchedules_SubjectCode",
                table: "SubjectSchedule",
                newName: "IX_SubjectSchedule_SubjectCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectSchedule",
                table: "SubjectSchedule",
                column: "EDPCode");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSchedule_Subjects_SubjectCode",
                table: "SubjectSchedule",
                column: "SubjectCode",
                principalTable: "Subjects",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
