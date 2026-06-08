using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CipherLock.Migrations
{
    /// <inheritdoc />
    public partial class IsDeletedInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vaults_users_UserId",
                table: "vaults");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "credentials",
                newName: "Username");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "vaults",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_vaults_users_UserId",
                table: "vaults",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vaults_users_UserId",
                table: "vaults");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "credentials",
                newName: "PasswordHash");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "vaults",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_vaults_users_UserId",
                table: "vaults",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
