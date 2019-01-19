using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class Initialdw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YouTubeChannelThumbnailUrl",
                table: "VideoPost");

            migrationBuilder.AddColumn<string>(
                name: "YouTubeChannelThumbnailUrl",
                table: "Uploader",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YouTubeChannelThumbnailUrl",
                table: "Uploader");

            migrationBuilder.AddColumn<string>(
                name: "YouTubeChannelThumbnailUrl",
                table: "VideoPost",
                nullable: true);
        }
    }
}
