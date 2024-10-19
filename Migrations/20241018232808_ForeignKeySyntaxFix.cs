using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeySyntaxFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSchedule_Subjects_SSFEDPCODE",
                table: "SubjectSchedule");

            migrationBuilder.DropIndex(
                name: "IX_SubjectSchedule_SSFEDPCODE",
                table: "SubjectSchedule");

            migrationBuilder.DropColumn(
                name: "SSFEDPCODE",
                table: "SubjectSchedule");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectCode",
                table: "SubjectSchedule",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSchedule_SubjectCode",
                table: "SubjectSchedule",
                column: "SubjectCode");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSchedule_Subjects_SubjectCode",
                table: "SubjectSchedule",
                column: "SubjectCode",
                principalTable: "Subjects",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSchedule_Subjects_SubjectCode",
                table: "SubjectSchedule");

            migrationBuilder.DropIndex(
                name: "IX_SubjectSchedule_SubjectCode",
                table: "SubjectSchedule");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectCode",
                table: "SubjectSchedule",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "SSFEDPCODE",
                table: "SubjectSchedule",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSchedule_SSFEDPCODE",
                table: "SubjectSchedule",
                column: "SSFEDPCODE");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSchedule_Subjects_SSFEDPCODE",
                table: "SubjectSchedule",
                column: "SSFEDPCODE",
                principalTable: "Subjects",
                principalColumn: "Code");
        }
    }
}
