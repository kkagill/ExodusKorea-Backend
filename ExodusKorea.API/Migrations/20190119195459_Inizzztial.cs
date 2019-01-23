using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class Inizzztial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "JobsInDemand",
                newName: "TitleKR");

            migrationBuilder.AddColumn<string>(
                name: "TitleEN",
                table: "JobsInDemand",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleEN",
                table: "JobsInDemand");

            migrationBuilder.RenameColumn(
                name: "TitleKR",
                table: "JobsInDemand",
                newName: "Title");
        }
    }
}
