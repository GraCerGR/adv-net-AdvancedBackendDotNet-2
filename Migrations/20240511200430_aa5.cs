using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Handbook_Service.Migrations
{
    /// <inheritdoc />
    public partial class aa5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationDocumentTypes_EducationLevels_EducationLevelId",
                table: "EducationDocumentTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevels_EducationDocumentTypes_EducationDocumentTyp~",
                table: "EducationLevels");

            migrationBuilder.DropIndex(
                name: "IX_EducationLevels_EducationDocumentTypeModelId",
                table: "EducationLevels");

            migrationBuilder.DropColumn(
                name: "EducationDocumentTypeModelId",
                table: "EducationLevels");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationDocumentTypes_EducationLevels_EducationLevelId",
                table: "EducationDocumentTypes",
                column: "EducationLevelId",
                principalTable: "EducationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationDocumentTypes_EducationLevels_EducationLevelId",
                table: "EducationDocumentTypes");

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
                name: "FK_EducationDocumentTypes_EducationLevels_EducationLevelId",
                table: "EducationDocumentTypes",
                column: "EducationLevelId",
                principalTable: "EducationLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevels_EducationDocumentTypes_EducationDocumentTyp~",
                table: "EducationLevels",
                column: "EducationDocumentTypeModelId",
                principalTable: "EducationDocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
