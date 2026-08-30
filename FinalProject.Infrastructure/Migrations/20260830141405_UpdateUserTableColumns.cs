using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "age",
                table: "Users",
                newName: "Age");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameIndex(
                name: "IX_Users_username",
                table: "Users",
                newName: "IX_Users_UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "lastname");

            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Users",
                newName: "age");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "name");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserName",
                table: "Users",
                newName: "IX_Users_username");
        }
    }
}
