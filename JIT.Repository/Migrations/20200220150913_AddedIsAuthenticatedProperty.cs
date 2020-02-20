using Microsoft.EntityFrameworkCore.Migrations;

namespace JIT.Repository.Migrations
{
    public partial class AddedIsAuthenticatedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAuthenticated",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAuthenticated",
                table: "Users");
        }
    }
}
