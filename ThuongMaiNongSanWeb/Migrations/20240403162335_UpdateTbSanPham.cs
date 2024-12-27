using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateTbSanPham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpName",
                table: "SanPham",
                type: "nvarchar(450)",
                nullable: false
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
