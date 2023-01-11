using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrack.Migrations
{
    public partial class relationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "IssueReport",
                newName: "IssueId");

            migrationBuilder.AddColumn<string>(
                name: "BugUserId",
                table: "IssueReport",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_IssueReport_BugUserId",
                table: "IssueReport",
                column: "BugUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueReport_AspNetUsers_BugUserId",
                table: "IssueReport",
                column: "BugUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueReport_AspNetUsers_BugUserId",
                table: "IssueReport");

            migrationBuilder.DropIndex(
                name: "IX_IssueReport_BugUserId",
                table: "IssueReport");

            migrationBuilder.DropColumn(
                name: "BugUserId",
                table: "IssueReport");

            migrationBuilder.RenameColumn(
                name: "IssueId",
                table: "IssueReport",
                newName: "Id");
        }
    }
}
