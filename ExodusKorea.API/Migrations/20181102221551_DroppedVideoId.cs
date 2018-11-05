using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class DroppedVideoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "VideoCommentReply");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "VideoComment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoId",
                table: "VideoCommentReply",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoId",
                table: "VideoComment",
                nullable: true);
        }
    }
}
