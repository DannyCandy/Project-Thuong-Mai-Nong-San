using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class DelColNpp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SanPham_IDNPP",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "IDNPP",
                table: "SanPham");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NhaPhanPhoi",
                columns: table => new
                {
                    IDNPP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiaChiNPP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioDongCua = table.Column<TimeSpan>(type: "time(0)", nullable: false),
                    GioMoCua = table.Column<TimeSpan>(type: "time(0)", nullable: false),
                    HinhAnhNPP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNPP = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    TenNPP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaPhanPhoi", x => x.IDNPP);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tb.SanPham_Tb.NhaPhanPhoi",
                table: "SanPham",
                column: "IDNPP",
                principalTable: "NhaPhanPhoi",
                principalColumn: "IDNPP");
            migrationBuilder.AddColumn<string>(
                name: "IDNPP",
                table: "SanPham",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_IDNPP",
                table: "SanPham",
                column: "IDNPP");
        }
    }
}
