using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class RemovedCellPhone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CellPhone",
                table: "PI_Etc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CellPhone",
                table: "PI_Etc",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
