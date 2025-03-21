using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial_fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PostTags");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PostTags",
                newName: "TagId");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "PostTags",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "PostTags",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags",
                columns: new[] { "PostId", "TagId" });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_TagId",
                table: "PostTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Posts_PostId",
                table: "PostTags",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Posts_PostId",
                table: "PostTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_TagId",
                table: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "PostTags");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "PostTags",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PostTags",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PostTags",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags",
                column: "Id");
        }
    }
}
