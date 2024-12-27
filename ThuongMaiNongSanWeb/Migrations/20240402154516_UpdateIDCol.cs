using Microsoft.EntityFrameworkCore.Migrations;
using WebApp.Models;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateIDCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb.OrderDetail_Tb.SanPham", // Tên của ràng buộc khóa ngoại cần xóa
                table: "OrderDetail" // Bảng chứa ràng buộc khóa ngoại
            );
            // Thêm cột mới IdProduct với kiểu dữ liệu string và mặc định là GUID
            migrationBuilder.AddColumn<string>(
                name: "NewIdProduct",
                table: "SanPham",
                type: "nvarchar(450)",
                nullable: false,
                defaultValueSql: "NEWID()");
            //Xóa Primary cũ
            migrationBuilder.DropPrimaryKey(
                name: "PK_SanPham",
                table: "SanPham");

            // Xóa cột "Id" hiện tại
            migrationBuilder.DropColumn(
                name: "IDSP",
                table: "SanPham");

            // Đổi tên cột mới thành "Id"
            migrationBuilder.RenameColumn(
                name: "NewIdProduct",
                table: "SanPham",
                newName: "IDSP");

            // Đặt cột "Id" làm khóa chính
            migrationBuilder.AddPrimaryKey(
                name: "PK_SanPham",
                table: "SanPham",
                column: "IDSP");

            migrationBuilder.AddForeignKey(
                        name: "FK_Tb.OrderDetail_Tb.SanPham",
                        table: "OrderDetail",
                        column: "IDSp",
                        principalTable: "SanPham",
                        principalColumn: "IDSP",
                        onDelete: ReferentialAction.Cascade, // Hành động khi dữ liệu tham chiếu bị xóa
                        onUpdate: ReferentialAction.Cascade // Hành động khi dữ liệu tham chiếu được cập nhật
            );


            //-------------------------------------------------------------------------------------------
            migrationBuilder.DropForeignKey(
                name: "FK_Tb.SanPham_Tb.ChungNhan", // Tên của ràng buộc khóa ngoại cần xóa
                table: "SanPham" // Bảng chứa ràng buộc khóa ngoại
            );
            // Thêm cột mới IdProduct với kiểu dữ liệu string và mặc định là GUID
            migrationBuilder.AddColumn<string>(
                name: "NewIdProduct",
                table: "ChungNhan",
                type: "nvarchar(450)",
                nullable: false,
                defaultValueSql: "NEWID()");
            //Xóa Primary cũ
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChungNhan",
                table: "ChungNhan");

            // Xóa cột "Id" hiện tại
            migrationBuilder.DropColumn(
                name: "IDChungNhan",
                table: "ChungNhan");

            // Đổi tên cột mới thành "Id"
            migrationBuilder.RenameColumn(
                name: "NewIdProduct",
                table: "ChungNhan",
                newName: "IDChungNhan");

            // Đặt cột "Id" làm khóa chính
            migrationBuilder.AddPrimaryKey(
                name: "PK_ChungNhan",
                table: "ChungNhan",
                column: "IDChungNhan");


            migrationBuilder.AddForeignKey(
                        name: "FK_Tb.SanPham_Tb.ChungNhan",
                        table: "SanPham",
                        column: "IDChungNhan",
                        principalTable: "ChungNhan",
                        principalColumn: "IDChungNhan",
                        onDelete: ReferentialAction.Cascade, // Hành động khi dữ liệu tham chiếu bị xóa
                        onUpdate: ReferentialAction.Cascade // Hành động khi dữ liệu tham chiếu được cập nhật
            );

            //----------------------------------------------------------------------------------
            migrationBuilder.DropForeignKey(
                name: "FK_Tb.SanPham_Tb.NhaPhanPhoi", // Tên của ràng buộc khóa ngoại cần xóa
                table: "SanPham" // Bảng chứa ràng buộc khóa ngoại
            );
            // Thêm cột mới IdProduct với kiểu dữ liệu string và mặc định là GUID
            migrationBuilder.AddColumn<string>(
                name: "NewIdProduct",
                table: "NhaPhanPhoi",
                type: "nvarchar(450)",
                nullable: false,
                defaultValueSql: "NEWID()");
            //Xóa Primary cũ
            migrationBuilder.DropPrimaryKey(
                name: "PK_NhaPhanPhoi",
                table: "NhaPhanPhoi");

            // Xóa cột "Id" hiện tại
            migrationBuilder.DropColumn(
                name: "IDNPP",
                table: "NhaPhanPhoi");

            // Đổi tên cột mới thành "Id"
            migrationBuilder.RenameColumn(
                name: "NewIdProduct",
                table: "NhaPhanPhoi",
                newName: "IDNPP");

            // Đặt cột "Id" làm khóa chính
            migrationBuilder.AddPrimaryKey(
                name: "PK_NhaPhanPhoi",
                table: "NhaPhanPhoi",
                column: "IDNPP");
            migrationBuilder.AddForeignKey(
                        name: "FK_Tb.SanPham_Tb.NhaPhanPhoi",
                        table: "SanPham",
                        column: "IDNPP",
                        principalTable: "NhaPhanPhoi",
                        principalColumn: "IDNPP",
                        onDelete: ReferentialAction.Cascade, // Hành động khi dữ liệu tham chiếu bị xóa
                        onUpdate: ReferentialAction.Cascade // Hành động khi dữ liệu tham chiếu được cập nhật
            );

            //----------------------------------------------------------------------------------
            migrationBuilder.DropForeignKey(
                name: "FK_Tb.SanPham_Tb.Category", // Tên của ràng buộc khóa ngoại cần xóa
                table: "SanPham" // Bảng chứa ràng buộc khóa ngoại
            );
            // Thêm cột mới IdProduct với kiểu dữ liệu string và mặc định là GUID
            migrationBuilder.AddColumn<string>(
                name: "NewIdProduct",
                table: "Category",
                type: "nvarchar(450)",
                nullable: false,
                defaultValueSql: "NEWID()");
            //Xóa Primary cũ
            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            // Xóa cột "Id" hiện tại
            migrationBuilder.DropColumn(
                name: "IDCategory",
                table: "Category");

            // Đổi tên cột mới thành "Id"
            migrationBuilder.RenameColumn(
                name: "NewIdProduct",
                table: "Category",
                newName: "IDCategory");

            // Đặt cột "Id" làm khóa chính
            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "IDCategory");
            migrationBuilder.AddForeignKey(
                        name: "FK_Tb.SanPham_Tb.Category",
                        table: "SanPham",
                        column: "IDCategory",
                        principalTable: "Category",
                        principalColumn: "IDCategory",
                        onDelete: ReferentialAction.Cascade, // Hành động khi dữ liệu tham chiếu bị xóa
                        onUpdate: ReferentialAction.Cascade // Hành động khi dữ liệu tham chiếu được cập nhật
            );

            //----------------------------------------------------------------------------------
            
            // Thêm cột mới IdProduct với kiểu dữ liệu string và mặc định là GUID
            migrationBuilder.AddColumn<string>(
                name: "NewIdProduct",
                table: "OrderDetail",
                type: "nvarchar(450)",
                nullable: false,
                defaultValueSql: "NEWID()");
            //Xóa Primary cũ
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail");

            // Xóa cột "Id" hiện tại
            migrationBuilder.DropColumn(
                name: "OrderDetailID",
                table: "OrderDetail");

            // Đổi tên cột mới thành "Id"
            migrationBuilder.RenameColumn(
                name: "NewIdProduct",
                table: "OrderDetail",
                newName: "OrderDetailID");

            // Đặt cột "Id" làm khóa chính
            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail",
                column: "OrderDetailID");
            
            //------------------------------------------------------------------------------------------

            // Thêm cột mới IdProduct với kiểu dữ liệu string và mặc định là GUID
            migrationBuilder.AddColumn<string>(
                name: "NewIdProduct",
                table: "ThongTinLienHe",
                type: "nvarchar(450)",
                nullable: false,
                defaultValueSql: "NEWID()");
            //Xóa Primary cũ
            migrationBuilder.DropPrimaryKey(
                name: "PK_ThongTinLienHe",
                table: "ThongTinLienHe");

            // Xóa cột "Id" hiện tại
            migrationBuilder.DropColumn(
                name: "IDTTLH",
                table: "ThongTinLienHe");

            // Đổi tên cột mới thành "Id"
            migrationBuilder.RenameColumn(
                name: "NewIdProduct",
                table: "ThongTinLienHe",
                newName: "IDTTLH");

            // Đặt cột "Id" làm khóa chính
            migrationBuilder.AddPrimaryKey(
                name: "PK_ThongTinLienHe",
                table: "ThongTinLienHe",
                column: "IDTTLH");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_AspNetUsers_UserID",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ThongTinLienHe_AspNetUsers_UserId",
                table: "ThongTinLienHe");

            migrationBuilder.DropIndex(
                name: "IX_ThongTinLienHe_UserId",
                table: "ThongTinLienHe");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_UserID",
                table: "OrderDetail");
        }
    }
}
