using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPMVC.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class a1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {





            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ShoeSizeId",
                table: "OrderDetails",
                column: "ShoeSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeader_ApplicationUserId",
                table: "OrderHeaders",
                column: "ApplicationUserId");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
