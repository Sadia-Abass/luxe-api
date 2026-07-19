using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace luxe.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpadteRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByIp",
                schema: "dbo",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "RevokedByIp",
                schema: "dbo",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "Expires",
                schema: "dbo",
                table: "RefreshTokens",
                newName: "ExpireDate");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                schema: "dbo",
                table: "RefreshTokens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                schema: "dbo",
                table: "RefreshTokens",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_Token",
                schema: "dbo",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "ExpireDate",
                schema: "dbo",
                table: "RefreshTokens",
                newName: "Expires");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                schema: "dbo",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByIp",
                schema: "dbo",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevokedByIp",
                schema: "dbo",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
