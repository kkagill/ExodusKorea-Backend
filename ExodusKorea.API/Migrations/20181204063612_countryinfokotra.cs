using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class countryinfokotra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoPost_Category_CategoryId",
                table: "VideoPost");

            migrationBuilder.DropTable(
                name: "ImmigrationVisa");

            migrationBuilder.DropTable(
                name: "LivingCondition");

            migrationBuilder.DropTable(
                name: "PromisingField");

            migrationBuilder.DropTable(
                name: "SettlementGuide");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "VideoPost",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CountryInfoKOTRA",
                columns: table => new
                {
                    CountryInfoKOTRAId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountryId = table.Column<int>(nullable: false),
                    ImmigrationVisa = table.Column<string>(nullable: true),
                    LivingCondition = table.Column<string>(nullable: true),
                    PromosingField = table.Column<string>(nullable: true),
                    SettlementGuide = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryInfoKOTRA", x => x.CountryInfoKOTRAId);
                    table.ForeignKey(
                        name: "FK_CountryInfoKOTRA_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryInfoKOTRA_CountryId",
                table: "CountryInfoKOTRA",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPost_Category_CategoryId",
                table: "VideoPost",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoPost_Category_CategoryId",
                table: "VideoPost");

            migrationBuilder.DropTable(
                name: "CountryInfoKOTRA");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "VideoPost",
                nullable: true,
                oldClrType: typeof(int));

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

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPost_Category_CategoryId",
                table: "VideoPost",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
