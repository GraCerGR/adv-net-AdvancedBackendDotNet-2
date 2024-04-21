using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Document_Service.Migrations
{
    /// <inheritdoc />
    public partial class Documents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    PathToFile = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentTypes = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PassportFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    PathToFile = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeriesNumber = table.Column<string>(type: "text", nullable: false),
                    Birthplace = table.Column<string>(type: "text", nullable: false),
                    WhenIssued = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IssuedByWhom = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassportFiles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationFiles");

            migrationBuilder.DropTable(
                name: "PassportFiles");
        }
    }
}
