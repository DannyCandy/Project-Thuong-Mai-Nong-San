using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class AddFieldInDaiLyOnline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarDaiLy",
                table: "DaiLyOnline",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Confirm",
                table: "DaiLyOnline",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GioiThieu",
                table: "DaiLyOnline",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarDaiLy",
                table: "DaiLyOnline");

            migrationBuilder.DropColumn(
                name: "Confirm",
                table: "DaiLyOnline");

            migrationBuilder.DropColumn(
                name: "GioiThieu",
                table: "DaiLyOnline");
        }
    }
}
