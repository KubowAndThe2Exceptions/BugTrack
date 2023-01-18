using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrack.Migrations
{
    public partial class OwnerAndTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Profiles",
                newName: "UserJobTitle");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Profiles",
                newName: "OwnerName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserJobTitle",
                table: "Profiles",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "OwnerName",
                table: "Profiles",
                newName: "FirstName");
        }
    }
}
