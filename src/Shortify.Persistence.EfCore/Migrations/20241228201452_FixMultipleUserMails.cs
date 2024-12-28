using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortify.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class FixMultipleUserMails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Urls_UserMail",
                table: "Urls");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_UserMail",
                table: "Urls",
                column: "UserMail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Urls_UserMail",
                table: "Urls");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_UserMail",
                table: "Urls",
                column: "UserMail",
                unique: true);
        }
    }
}
