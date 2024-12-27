using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Bảng ChungNhan
            migrationBuilder.DropColumn(
                name: "HinhAnhChungNhan",
                table: "ChungNhan");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnhChungNhan",
                table: "ChungNhan", 
                nullable: true);

            //Bảng NhaPhanPhoi
            migrationBuilder.DropColumn(
                name: "HinhAnhNPP",
                table: "NhaPhanPhoi");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnhNPP",
                table: "NhaPhanPhoi",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
