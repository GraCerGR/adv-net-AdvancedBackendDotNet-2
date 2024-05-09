using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Handbook_Service.Migrations
{
    /// <inheritdoc />
    public partial class documentTypes5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevels_EducationDocumentTypes_EducationDocumentTyp~",
                table: "EducationLevels");

            migrationBuilder.DropIndex(
                name: "IX_EducationLevels_EducationDocumentTypeModelId",
                table: "EducationLevels");

            migrationBuilder.DropColumn(
                name: "EducationDocumentTypeModelId",
                table: "EducationLevels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EducationDocumentTypeModelId",
                table: "EducationLevels",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevels_EducationDocumentTypeModelId",
                table: "EducationLevels",
                column: "EducationDocumentTypeModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevels_EducationDocumentTypes_EducationDocumentTyp~",
                table: "EducationLevels",
                column: "EducationDocumentTypeModelId",
                principalTable: "EducationDocumentTypes",
                principalColumn: "Id");
        }
    }
}
