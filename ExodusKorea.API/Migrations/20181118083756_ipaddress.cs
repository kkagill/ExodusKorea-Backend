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
                name: "IpAddress",
                table: "MinimumCostOfLiving",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "MinimumCostOfLiving",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "MinimumCostOfLiving");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "MinimumCostOfLiving");
        }
    }
}
