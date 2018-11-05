using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class AddedAuthorColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorDisplayName",
                table: "VideoCommentReply",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorDisplayName",
                table: "VideoComment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorDisplayName",
                table: "VideoCommentReply");

            migrationBuilder.DropColumn(
                name: "AuthorDisplayName",
                table: "VideoComment");
        }
    }
}
