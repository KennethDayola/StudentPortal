using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudId",
                table: "EnrollmentHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_EnrollmentHeaders_EnrollmentHeaderStudId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "EnrollmentHeaderStudId",
                table: "Students",
                newName: "EnrollmentHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_EnrollmentHeaderStudId",
                table: "Students",
                newName: "IX_Students_EnrollmentHeaderId");

            migrationBuilder.RenameColumn(
                name: "StudId",
                table: "EnrollmentHeaders",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentHeaders_Students_Id",
                table: "EnrollmentHeaders",
                column: "Id",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_EnrollmentHeaders_EnrollmentHeaderId",
                table: "Students",
                column: "EnrollmentHeaderId",
                principalTable: "EnrollmentHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentHeaders_Students_Id",
                table: "EnrollmentHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_EnrollmentHeaders_EnrollmentHeaderId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "EnrollmentHeaderId",
                table: "Students",
                newName: "EnrollmentHeaderStudId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_EnrollmentHeaderId",
                table: "Students",
                newName: "IX_Students_EnrollmentHeaderStudId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EnrollmentHeaders",
                newName: "StudId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudId",
                table: "EnrollmentHeaders",
                column: "StudId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_EnrollmentHeaders_EnrollmentHeaderStudId",
                table: "Students",
                column: "EnrollmentHeaderStudId",
                principalTable: "EnrollmentHeaders",
                principalColumn: "StudId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
