using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class myvideos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyVideo",
                columns: table => new
                {
                    MyVideosId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VideoPostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyVideo", x => x.MyVideosId);
                    table.ForeignKey(
                        name: "FK_MyVideo_VideoPost_VideoPostId",
                        column: x => x.VideoPostId,
                        principalTable: "VideoPost",
                        principalColumn: "VideoPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyVideo_VideoPostId",
                table: "MyVideo",
                column: "VideoPostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyVideo");
        }
    }
}
