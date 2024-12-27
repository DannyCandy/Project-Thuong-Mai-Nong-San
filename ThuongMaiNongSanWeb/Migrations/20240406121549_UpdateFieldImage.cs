using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateFieldImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Bảng SanPham
            migrationBuilder.DropColumn(
                name: "HinhAnhSP",
                table: "SanPham");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnhSP",
                table: "SanPham",
                nullable: false,
                defaultValue: "");

            //Bảng ChungNhan
            migrationBuilder.DropColumn(
                name: "HinhAnhChungNhan",
                table: "ChungNhan");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnhChungNhan",
                table: "ChungNhan",
                nullable: false,
                defaultValue: "");

            //Bảng NhaPhanPhoi
            migrationBuilder.AddColumn<string>(
                name: "HinhAnhNPP",
                table: "NhaPhanPhoi",
                nullable: false,
                defaultValue: "");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Bảng SanPham
            migrationBuilder.DropColumn(
                name: "HinhAnhSP",
                table: "SanPham");

            migrationBuilder.AddColumn<byte[]>(
                name: "HinhAnhSP",
                table: "SanPham",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: Array.Empty<byte>());

            //Bảng ChungNhan
            migrationBuilder.DropColumn(
                name: "HinhAnhChungNhan",
                table: "ChungNhan");

            migrationBuilder.AddColumn<byte[]>(
                name: "HinhAnhChungNhan",
                table: "ChungNhan",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: Array.Empty<byte>());

            //Bảng NhaPhanPhoi
            migrationBuilder.DropColumn(
                name: "HinhAnhNPP",
                table: "NhaPhanPhoi");

        }
    }
}
