using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class RenamePrerequisite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prerequistes_Subjects_SubjectCode",
                table: "Prerequistes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prerequistes",
                table: "Prerequistes");

            migrationBuilder.RenameTable(
                name: "Prerequistes",
                newName: "Prerequisites");

            migrationBuilder.RenameIndex(
                name: "IX_Prerequistes_SubjectCode",
                table: "Prerequisites",
                newName: "IX_Prerequisites_SubjectCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prerequisites",
                table: "Prerequisites",
                columns: new[] { "SubjectCode", "PreCode" });

            migrationBuilder.AddForeignKey(
                name: "FK_Prerequisites_Subjects_SubjectCode",
                table: "Prerequisites",
                column: "SubjectCode",
                principalTable: "Subjects",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prerequisites_Subjects_SubjectCode",
                table: "Prerequisites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prerequisites",
                table: "Prerequisites");

            migrationBuilder.RenameTable(
                name: "Prerequisites",
                newName: "Prerequistes");

            migrationBuilder.RenameIndex(
                name: "IX_Prerequisites_SubjectCode",
                table: "Prerequistes",
                newName: "IX_Prerequistes_SubjectCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prerequistes",
                table: "Prerequistes",
                columns: new[] { "SubjectCode", "PreCode" });

            migrationBuilder.AddForeignKey(
                name: "FK_Prerequistes_Subjects_SubjectCode",
                table: "Prerequistes",
                column: "SubjectCode",
                principalTable: "Subjects",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
