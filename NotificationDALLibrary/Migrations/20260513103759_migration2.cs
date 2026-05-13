using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationDALLibrary.Migrations
{
    /// <inheritdoc />
    public partial class migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserEmail",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserEmail",
                table: "Notifications",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserEmail",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserEmail",
                table: "Notifications",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
