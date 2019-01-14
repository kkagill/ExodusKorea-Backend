using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class Initialzz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VimeoId",
                table: "VideoPost");

            migrationBuilder.DropColumn(
                name: "VimeoId",
                table: "Notification");

            migrationBuilder.AddColumn<byte>(
                name: "IsGoogleDriveVideo",
                table: "VideoPost",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "IsGoogleDriveVideo",
                table: "Notification",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGoogleDriveVideo",
                table: "VideoPost");

            migrationBuilder.DropColumn(
                name: "IsGoogleDriveVideo",
                table: "Notification");

            migrationBuilder.AddColumn<long>(
                name: "VimeoId",
                table: "VideoPost",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "VimeoId",
                table: "Notification",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
