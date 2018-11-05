using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class NewColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewVideoId",
                table: "VideoComment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VideoComment_NewVideoId",
                table: "VideoComment",
                column: "NewVideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoComment_NewVideo_NewVideoId",
                table: "VideoComment",
                column: "NewVideoId",
                principalTable: "NewVideo",
                principalColumn: "NewVideoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoComment_NewVideo_NewVideoId",
                table: "VideoComment");

            migrationBuilder.DropIndex(
                name: "IX_VideoComment_NewVideoId",
                table: "VideoComment");

            migrationBuilder.DropColumn(
                name: "NewVideoId",
                table: "VideoComment");
        }
    }
}
