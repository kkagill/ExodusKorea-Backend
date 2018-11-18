using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class AddedCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "VideoCommentReply",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "VideoComment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "VideoCommentReply");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "VideoComment");
        }
    }
}
