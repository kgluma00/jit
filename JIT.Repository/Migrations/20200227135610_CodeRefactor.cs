using Microsoft.EntityFrameworkCore.Migrations;

namespace JIT.Repository.Migrations
{
    public partial class CodeRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAuthenticated",
                table: "Users",
                newName: "IsAuthenticated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAuthenticated",
                table: "Users",
                newName: "isAuthenticated");
        }
    }
}
