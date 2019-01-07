using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class salaryinfoidnull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoPost_SalaryInfo_SalaryInfoId",
                table: "VideoPost");

            migrationBuilder.AlterColumn<int>(
                name: "SalaryInfoId",
                table: "VideoPost",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPost_SalaryInfo_SalaryInfoId",
                table: "VideoPost",
                column: "SalaryInfoId",
                principalTable: "SalaryInfo",
                principalColumn: "SalaryInfoId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoPost_SalaryInfo_SalaryInfoId",
                table: "VideoPost");

            migrationBuilder.AlterColumn<int>(
                name: "SalaryInfoId",
                table: "VideoPost",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPost_SalaryInfo_SalaryInfoId",
                table: "VideoPost",
                column: "SalaryInfoId",
                principalTable: "SalaryInfo",
                principalColumn: "SalaryInfoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
