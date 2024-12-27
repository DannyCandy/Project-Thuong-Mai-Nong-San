using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class CapNhatOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_Tb.OrderDetail_Tb.AspNetUsers",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "DCGiaoHang",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "OrderTime",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "OrderDetail");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "OrderDetail",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_UserID",
                table: "OrderDetail",
                newName: "IX_OrderDetail_OrderId");

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OrderTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DcGiaoHang = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserID",
                table: "Order",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb.OrderDetail_Tb.Order",
                table: "OrderDetail",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "OrderId");*/
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderDetail",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb.OrderDetail_Tb.Order",
                table: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderDetail",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_UserID");

            migrationBuilder.AddColumn<string>(
                name: "DCGiaoHang",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderTime",
                table: "OrderDetail",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "OrderDetail",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TotalPrice",
                table: "OrderDetail",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_AspNetUsers_UserID",
                table: "OrderDetail",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
