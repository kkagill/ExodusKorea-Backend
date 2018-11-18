using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class minimumcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MinimumCostOfLiving",
                columns: table => new
                {
                    MinimumCostOfLivingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cell = table.Column<decimal>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    CountryInfoId = table.Column<int>(nullable: false),
                    Etc = table.Column<string>(nullable: true),
                    Food = table.Column<decimal>(nullable: false),
                    Internet = table.Column<decimal>(nullable: false),
                    Rent = table.Column<decimal>(nullable: false),
                    Transportation = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinimumCostOfLiving", x => x.MinimumCostOfLivingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MinimumCostOfLiving");
        }
    }
}
