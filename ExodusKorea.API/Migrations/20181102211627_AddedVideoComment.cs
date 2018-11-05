using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExodusKorea.API.Migrations
{
    public partial class AddedVideoComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoComment",
                columns: table => new
                {
                    VideoCommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Likes = table.Column<int>(nullable: false),
                    VideoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoComment", x => x.VideoCommentId);
                });

            migrationBuilder.CreateTable(
                name: "VideoCommentReply",
                columns: table => new
                {
                    VideoCommentReplyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Likes = table.Column<int>(nullable: false),
                    VideoCommentId = table.Column<int>(nullable: false),
                    VideoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoCommentReply", x => x.VideoCommentReplyId);
                    table.ForeignKey(
                        name: "FK_VideoCommentReply_VideoComment_VideoCommentId",
                        column: x => x.VideoCommentId,
                        principalTable: "VideoComment",
                        principalColumn: "VideoCommentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoCommentReply_VideoCommentId",
                table: "VideoCommentReply",
                column: "VideoCommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoCommentReply");

            migrationBuilder.DropTable(
                name: "VideoComment");
        }
    }
}
