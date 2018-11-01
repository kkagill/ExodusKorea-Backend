using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class AddedPriceInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceInfo",
                columns: table => new
                {
                    PriceInfoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CostOfLivingIndex = table.Column<decimal>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    GroceriesIndex = table.Column<decimal>(nullable: false),
                    RentIndex = table.Column<decimal>(nullable: false),
                    RestaurantPriceIndex = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceInfo", x => x.PriceInfoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceInfo");
        }
    }
}
