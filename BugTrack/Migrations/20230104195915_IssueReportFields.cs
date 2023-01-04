using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrack.Migrations
{
    public partial class IssueReportFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "IssueReport",
                newName: "ReplicationDescription");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFound",
                table: "IssueReport",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "GeneralDescription",
                table: "IssueReport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IssueTitle",
                table: "IssueReport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFound",
                table: "IssueReport");

            migrationBuilder.DropColumn(
                name: "GeneralDescription",
                table: "IssueReport");

            migrationBuilder.DropColumn(
                name: "IssueTitle",
                table: "IssueReport");

            migrationBuilder.RenameColumn(
                name: "ReplicationDescription",
                table: "IssueReport",
                newName: "Description");
        }
    }
}
