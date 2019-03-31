using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class Inasditial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoPost_Career_CareerId",
                table: "VideoPost");

            migrationBuilder.AlterColumn<int>(
                name: "CareerId",
                table: "VideoPost",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPost_Career_CareerId",
                table: "VideoPost",
                column: "CareerId",
                principalTable: "Career",
                principalColumn: "CareerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoPost_Career_CareerId",
                table: "VideoPost");

            migrationBuilder.AlterColumn<int>(
                name: "CareerId",
                table: "VideoPost",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPost_Career_CareerId",
                table: "VideoPost",
                column: "CareerId",
                principalTable: "Career",
                principalColumn: "CareerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
