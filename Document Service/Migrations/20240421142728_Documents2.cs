using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Document_Service.Migrations
{
    /// <inheritdoc />
    public partial class Documents2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "PassportFiles");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "EducationFiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "PassportFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "EducationFiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
