using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class logexceptionsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Log_HttpResponseException",
                columns: table => new
                {
                    HttpResponseExceptionId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Error = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    PageUrl = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log_HttpResponseException", x => x.HttpResponseExceptionId);
                });

            migrationBuilder.CreateTable(
                name: "Log_SiteException",
                columns: table => new
                {
                    SiteExceptionId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Exception = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    PageUrl = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log_SiteException", x => x.SiteExceptionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log_HttpResponseException");

            migrationBuilder.DropTable(
                name: "Log_SiteException");
        }
    }
}
