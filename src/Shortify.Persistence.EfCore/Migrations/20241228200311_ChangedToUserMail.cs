using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortify.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class ChangedToUserMail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Urls");

            migrationBuilder.AddColumn<string>(
                name: "UserMail",
                table: "Urls",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Urls_UserMail",
                table: "Urls",
                column: "UserMail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Urls_UserMail",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "UserMail",
                table: "Urls");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Urls",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
