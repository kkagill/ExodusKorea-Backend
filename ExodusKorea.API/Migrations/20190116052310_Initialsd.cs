using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class Initialsd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoPost_Uploader_UploaderId",
                table: "VideoPost");

            migrationBuilder.AlterColumn<int>(
                name: "UploaderId",
                table: "VideoPost",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPost_Uploader_UploaderId",
                table: "VideoPost",
                column: "UploaderId",
                principalTable: "Uploader",
                principalColumn: "UploaderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoPost_Uploader_UploaderId",
                table: "VideoPost");

            migrationBuilder.AlterColumn<int>(
                name: "UploaderId",
                table: "VideoPost",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPost_Uploader_UploaderId",
                table: "VideoPost",
                column: "UploaderId",
                principalTable: "Uploader",
                principalColumn: "UploaderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
