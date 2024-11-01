using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompositeKeyBack : Migration
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
                columns: new[] { "SubjectCode", "PreCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Prerequistes_SubjectCode",
                table: "Prerequistes",
                column: "SubjectCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Prerequistes",
                table: "Prerequistes");

            migrationBuilder.DropIndex(
                name: "IX_Prerequistes_SubjectCode",
                table: "Prerequistes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prerequistes",
                table: "Prerequistes",
                column: "SubjectCode");
        }
    }
}
