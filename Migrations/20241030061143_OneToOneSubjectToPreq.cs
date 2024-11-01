using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class OneToOneSubjectToPreq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Prerequistes",
                table: "Prerequistes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prerequistes",
                table: "Prerequistes",
                column: "SubjectCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Prerequistes",
                table: "Prerequistes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prerequistes",
                table: "Prerequistes",
                columns: new[] { "SubjectCode", "PreCode" });
        }
    }
}
