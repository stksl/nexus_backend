using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class namingfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "expires_date",
                table: "RefreshTokens",
                newName: "Expires");

            migrationBuilder.RenameColumn(
                name: "headline",
                table: "Posts",
                newName: "Headline");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Posts",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "last_modified",
                table: "Posts",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "date_created",
                table: "Posts",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "repost_date",
                table: "PostReposts",
                newName: "RepostDate");

            migrationBuilder.RenameColumn(
                name: "last_modified",
                table: "Comments",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "date_created",
                table: "Comments",
                newName: "DateCreated");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expires",
                table: "RefreshTokens",
                newName: "expires_date");

            migrationBuilder.RenameColumn(
                name: "Headline",
                table: "Posts",
                newName: "headline");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Posts",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "Posts",
                newName: "last_modified");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Posts",
                newName: "date_created");

            migrationBuilder.RenameColumn(
                name: "RepostDate",
                table: "PostReposts",
                newName: "repost_date");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "Comments",
                newName: "last_modified");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Comments",
                newName: "date_created");
        }
    }
}
