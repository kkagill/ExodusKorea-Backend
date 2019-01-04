using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class ddd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImmigrationVisa",
                columns: table => new
                {
                    ImmigrationVisaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImmigrationVisa", x => x.ImmigrationVisaId);
                });

            migrationBuilder.CreateTable(
                name: "LivingCondition",
                columns: table => new
                {
                    LivingConditionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivingCondition", x => x.LivingConditionId);
                });

            migrationBuilder.CreateTable(
                name: "PromisingField",
                columns: table => new
                {
                    PromisingFieldId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromisingField", x => x.PromisingFieldId);
                });

            migrationBuilder.CreateTable(
                name: "SettlementGuide",
                columns: table => new
                {
                    SettlementGuideId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettlementGuide", x => x.SettlementGuideId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImmigrationVisa");

            migrationBuilder.DropTable(
                name: "LivingCondition");

            migrationBuilder.DropTable(
                name: "PromisingField");

            migrationBuilder.DropTable(
                name: "SettlementGuide");
        }
    }
}
