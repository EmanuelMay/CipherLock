using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CipherLock.Migrations
{
    /// <inheritdoc />
    public partial class RemovendoIsDeletedOfUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
