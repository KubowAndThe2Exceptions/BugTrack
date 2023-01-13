using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrack.Migrations
{
    public partial class CommentProfileRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueReport_AspNetUsers_BugUserId",
                table: "IssueReport");

            migrationBuilder.AlterColumn<string>(
                name: "BugUserId",
                table: "IssueReport",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimePosted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BugUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IssueReportEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_BugUserId",
                        column: x => x.BugUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Comments_IssueReport_IssueReportEntityId",
                        column: x => x.IssueReportEntityId,
                        principalTable: "IssueReport",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BugUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_AspNetUsers_BugUserId",
                        column: x => x.BugUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BugUserId",
                table: "Comments",
                column: "BugUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IssueReportEntityId",
                table: "Comments",
                column: "IssueReportEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_BugUserId",
                table: "Profiles",
                column: "BugUserId",
                unique: true,
                filter: "[BugUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueReport_AspNetUsers_BugUserId",
                table: "IssueReport",
                column: "BugUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueReport_AspNetUsers_BugUserId",
                table: "IssueReport");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.AlterColumn<string>(
                name: "BugUserId",
                table: "IssueReport",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IssueReport_AspNetUsers_BugUserId",
                table: "IssueReport",
                column: "BugUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
