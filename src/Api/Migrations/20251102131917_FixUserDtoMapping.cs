using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bootler.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixUserDtoMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_Users_AsignedById",
                table: "UserTasks");

            migrationBuilder.RenameColumn(
                name: "AsignedById",
                table: "UserTasks",
                newName: "AssignedById");

            migrationBuilder.RenameIndex(
                name: "IX_UserTasks_AsignedById",
                table: "UserTasks",
                newName: "IX_UserTasks_AssignedById");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_Users_AssignedById",
                table: "UserTasks",
                column: "AssignedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_Users_AssignedById",
                table: "UserTasks");

            migrationBuilder.RenameColumn(
                name: "AssignedById",
                table: "UserTasks",
                newName: "AsignedById");

            migrationBuilder.RenameIndex(
                name: "IX_UserTasks_AssignedById",
                table: "UserTasks",
                newName: "IX_UserTasks_AsignedById");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_Users_AsignedById",
                table: "UserTasks",
                column: "AsignedById",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
