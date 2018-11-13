using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class AddedSalaryInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalaryInfo",
                columns: table => new
                {
                    SalaryInfoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Country = table.Column<string>(nullable: true),
                    High = table.Column<decimal>(nullable: false),
                    IsDisplayable = table.Column<bool>(nullable: false),
                    Low = table.Column<decimal>(nullable: false),
                    Median = table.Column<decimal>(nullable: false),
                    Occupation = table.Column<string>(nullable: true),
                    VideoPostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryInfo", x => x.SalaryInfoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryInfo");
        }
    }
}
