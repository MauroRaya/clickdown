using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clickdown.Migrations
{
    /// <inheritdoc />
    public partial class NewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Jobs",
                newName: "WorkerId");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Jobs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "Money",
                table: "Jobs",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "WorkerId",
                table: "Jobs",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
