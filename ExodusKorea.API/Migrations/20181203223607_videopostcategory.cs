using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class videopostcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "VideoPost",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VideoPost_CategoryId",
                table: "VideoPost",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPost_Category_CategoryId",
                table: "VideoPost",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoPost_Category_CategoryId",
                table: "VideoPost");

            migrationBuilder.DropIndex(
                name: "IX_VideoPost_CategoryId",
                table: "VideoPost");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "VideoPost");
        }
    }
}
