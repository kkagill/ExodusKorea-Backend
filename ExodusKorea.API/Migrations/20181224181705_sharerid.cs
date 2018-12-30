using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class sharerid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SharerId",
                table: "VideoPost",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Log_HttpResponseException",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SharerId",
                table: "VideoPost");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Log_HttpResponseException",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
