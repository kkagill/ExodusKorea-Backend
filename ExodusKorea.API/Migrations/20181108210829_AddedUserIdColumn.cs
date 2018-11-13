using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class AddedUserIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "VideoCommentReply",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "VideoComment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "VideoCommentReply");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "VideoComment");
        }
    }
}
