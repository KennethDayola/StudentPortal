using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class TestTry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentHeaders_Students_Id",
                table: "EnrollmentHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_EnrollmentHeaders_EnrollmentHeaderId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_EnrollmentHeaderId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EnrollmentHeaderId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EnrollmentHeaders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "EnrollmentHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentHeaders_StudentId",
                table: "EnrollmentHeaders",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudentId",
                table: "EnrollmentHeaders",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudentId",
                table: "EnrollmentHeaders");

            migrationBuilder.DropIndex(
                name: "IX_EnrollmentHeaders_StudentId",
                table: "EnrollmentHeaders");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "EnrollmentHeaders");

            migrationBuilder.AddColumn<int>(
                name: "EnrollmentHeaderId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EnrollmentHeaders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Students_EnrollmentHeaderId",
                table: "Students",
                column: "EnrollmentHeaderId");

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
    }
}
