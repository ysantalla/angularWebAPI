using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "relCommentLikes");

            migrationBuilder.DropTable(
                name: "relIdeaFavorites");

            migrationBuilder.DropTable(
                name: "relIdeaLikes");

            migrationBuilder.DropTable(
                name: "relIdeaTags");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Ideas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ideas",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Article = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    HVersion = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Language = table.Column<int>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    ModifierId = table.Column<long>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    ViewCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ideas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ideas_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    HVersion = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Language = table.Column<int>(nullable: false),
                    ModifierId = table.Column<long>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    HVersion = table.Column<int>(nullable: false),
                    IdeaId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    ModifierId = table.Column<long>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "relIdeaFavorites",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    HVersion = table.Column<int>(nullable: false),
                    IdeaId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifierId = table.Column<long>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    Value = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relIdeaFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_relIdeaFavorites_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_relIdeaFavorites_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "relIdeaLikes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    HVersion = table.Column<int>(nullable: false),
                    IdeaId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifierId = table.Column<long>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    Vote = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relIdeaLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_relIdeaLikes_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_relIdeaLikes_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "relIdeaTags",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    HVersion = table.Column<int>(nullable: false),
                    IdeaId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifierId = table.Column<long>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    TagId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relIdeaTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_relIdeaTags_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_relIdeaTags_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_relIdeaTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "relCommentLikes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommentId = table.Column<long>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    HVersion = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifierId = table.Column<long>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    Vote = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relCommentLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_relCommentLikes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_relCommentLikes_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatorId",
                table: "Comments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdeaId",
                table: "Comments",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_CreatorId",
                table: "Ideas",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_relCommentLikes_CommentId",
                table: "relCommentLikes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_relCommentLikes_CreatorId",
                table: "relCommentLikes",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_relIdeaFavorites_CreatorId",
                table: "relIdeaFavorites",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_relIdeaFavorites_IdeaId",
                table: "relIdeaFavorites",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_relIdeaLikes_CreatorId",
                table: "relIdeaLikes",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_relIdeaLikes_IdeaId",
                table: "relIdeaLikes",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_relIdeaTags_CreatorId",
                table: "relIdeaTags",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_relIdeaTags_IdeaId",
                table: "relIdeaTags",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_relIdeaTags_TagId",
                table: "relIdeaTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CreatorId",
                table: "Tags",
                column: "CreatorId");
        }
    }
}
