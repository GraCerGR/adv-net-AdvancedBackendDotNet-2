using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Service.Migrations
{
    /// <inheritdoc />
    public partial class RevorkedTokensAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokensRevoked",
                table: "RefreshTokensRevoked");

            migrationBuilder.DropColumn(
                name: "Expires",
                table: "RefreshTokensRevoked");

            migrationBuilder.RenameTable(
                name: "RefreshTokensRevoked",
                newName: "AccessTokensRevoked");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "AccessTokensRevoked",
                newName: "AccessToken");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessTokensRevoked",
                table: "AccessTokensRevoked",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessTokensRevoked",
                table: "AccessTokensRevoked");

            migrationBuilder.RenameTable(
                name: "AccessTokensRevoked",
                newName: "RefreshTokensRevoked");

            migrationBuilder.RenameColumn(
                name: "AccessToken",
                table: "RefreshTokensRevoked",
                newName: "RefreshToken");

            migrationBuilder.AddColumn<DateTime>(
                name: "Expires",
                table: "RefreshTokensRevoked",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokensRevoked",
                table: "RefreshTokensRevoked",
                column: "Id");
        }
    }
}
