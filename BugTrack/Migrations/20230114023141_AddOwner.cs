using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrack.Migrations
{
    public partial class AddOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "Comments");
        }
    }
}
