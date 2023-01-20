using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrack.Migrations
{
    public partial class StatusModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModuleOrClass",
                table: "IssueReport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "IssueReport",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleOrClass",
                table: "IssueReport");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "IssueReport");
        }
    }
}
