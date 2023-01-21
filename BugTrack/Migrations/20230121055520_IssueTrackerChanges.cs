using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrack.Migrations
{
    public partial class IssueTrackerChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThreatLevel",
                table: "IssueReport",
                newName: "IssueThreatId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "IssueReport",
                newName: "IssueStatusId");

            migrationBuilder.AlterColumn<string>(
                name: "ModuleOrClass",
                table: "IssueReport",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IssueThreatId",
                table: "IssueReport",
                newName: "ThreatLevel");

            migrationBuilder.RenameColumn(
                name: "IssueStatusId",
                table: "IssueReport",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "ModuleOrClass",
                table: "IssueReport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
