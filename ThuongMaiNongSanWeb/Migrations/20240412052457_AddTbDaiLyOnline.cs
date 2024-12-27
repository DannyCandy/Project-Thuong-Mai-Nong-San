using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class AddTbDaiLyOnline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Confirm",
                table: "SanPham",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IdDaiLy",
                table: "SanPham",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DaiLyOnline",
                columns: table => new
                {
                    IDDaiLy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameDaiLy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaiLyOnline", x => x.IDDaiLy);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_IdDaiLy",
                table: "SanPham",
                column: "IdDaiLy");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb.SanPham_Tb.DaiLyOnline",
                table: "SanPham",
                column: "IdDaiLy",
                principalTable: "DaiLyOnline",
                principalColumn: "IDDaiLy");

            migrationBuilder.AlterColumn<string>(
                name: "IDNPP",
                table: "SanPham",
                nullable: true, // Cho phép cột nhận giá trị NULL
                oldNullable: false, // Khai báo trạng thái cũ của cột
                oldClrType: typeof(string)); // Khai báo kiểu dữ liệu cũ của cột
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb.SanPham_Tb.DaiLyOnline",
                table: "SanPham");

            migrationBuilder.DropTable(
                name: "DaiLyOnline");

            migrationBuilder.DropIndex(
                name: "IX_SanPham_IdDaiLy",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "Confirm",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "IdDaiLy",
                table: "SanPham");
        }
    }
}
