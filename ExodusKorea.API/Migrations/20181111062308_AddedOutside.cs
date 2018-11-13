using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class AddedOutside : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TwoBedRoom",
                table: "PI_Rent",
                newName: "TwoBedRoomOutside");

            migrationBuilder.RenameColumn(
                name: "OneBedRoom",
                table: "PI_Rent",
                newName: "TwoBedRoomCenter");

            migrationBuilder.AddColumn<decimal>(
                name: "OneBedRoomCenter",
                table: "PI_Rent",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OneBedRoomOutside",
                table: "PI_Rent",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OneBedRoomCenter",
                table: "PI_Rent");

            migrationBuilder.DropColumn(
                name: "OneBedRoomOutside",
                table: "PI_Rent");

            migrationBuilder.RenameColumn(
                name: "TwoBedRoomOutside",
                table: "PI_Rent",
                newName: "TwoBedRoom");

            migrationBuilder.RenameColumn(
                name: "TwoBedRoomCenter",
                table: "PI_Rent",
                newName: "OneBedRoom");
        }
    }
}
