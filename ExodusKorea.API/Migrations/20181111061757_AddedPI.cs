using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class AddedPI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PI_Etc",
                columns: table => new
                {
                    PI_EtcId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bus = table.Column<decimal>(nullable: false),
                    CellPhone = table.Column<decimal>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Gas = table.Column<decimal>(nullable: false),
                    Internet = table.Column<decimal>(nullable: false),
                    PriceInfoId = table.Column<int>(nullable: false),
                    Subway = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_Etc", x => x.PI_EtcId);
                    table.ForeignKey(
                        name: "FK_PI_Etc_PriceInfo_PriceInfoId",
                        column: x => x.PriceInfoId,
                        principalTable: "PriceInfo",
                        principalColumn: "PriceInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PI_Groceries",
                columns: table => new
                {
                    PI_GroceriesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Apple = table.Column<decimal>(nullable: false),
                    ChickenBreasts = table.Column<decimal>(nullable: false),
                    Cigarettes = table.Column<decimal>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Eggs = table.Column<decimal>(nullable: false),
                    Milk = table.Column<decimal>(nullable: false),
                    Potatoes = table.Column<decimal>(nullable: false),
                    PriceInfoId = table.Column<int>(nullable: false),
                    Water = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_Groceries", x => x.PI_GroceriesId);
                    table.ForeignKey(
                        name: "FK_PI_Groceries_PriceInfo_PriceInfoId",
                        column: x => x.PriceInfoId,
                        principalTable: "PriceInfo",
                        principalColumn: "PriceInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PI_Rent",
                columns: table => new
                {
                    PI_RentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    OneBedRoom = table.Column<decimal>(nullable: false),
                    PriceInfoId = table.Column<int>(nullable: false),
                    TwoBedRoom = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_Rent", x => x.PI_RentId);
                    table.ForeignKey(
                        name: "FK_PI_Rent_PriceInfo_PriceInfoId",
                        column: x => x.PriceInfoId,
                        principalTable: "PriceInfo",
                        principalColumn: "PriceInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PI_Restaurant",
                columns: table => new
                {
                    PI_RestaurantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BigMacMeal = table.Column<decimal>(nullable: false),
                    Cappuccino = table.Column<decimal>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    MealPerOne = table.Column<decimal>(nullable: false),
                    PriceInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_Restaurant", x => x.PI_RestaurantId);
                    table.ForeignKey(
                        name: "FK_PI_Restaurant_PriceInfo_PriceInfoId",
                        column: x => x.PriceInfoId,
                        principalTable: "PriceInfo",
                        principalColumn: "PriceInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PI_Etc_PriceInfoId",
                table: "PI_Etc",
                column: "PriceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_PI_Groceries_PriceInfoId",
                table: "PI_Groceries",
                column: "PriceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_PI_Rent_PriceInfoId",
                table: "PI_Rent",
                column: "PriceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_PI_Restaurant_PriceInfoId",
                table: "PI_Restaurant",
                column: "PriceInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PI_Etc");

            migrationBuilder.DropTable(
                name: "PI_Groceries");

            migrationBuilder.DropTable(
                name: "PI_Rent");

            migrationBuilder.DropTable(
                name: "PI_Restaurant");
        }
    }
}
