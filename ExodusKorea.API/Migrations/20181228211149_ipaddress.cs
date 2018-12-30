using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class ipaddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "VideoCommentReply",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "VideoComment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "VideoCommentReply");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "VideoComment");
        }
    }
}
