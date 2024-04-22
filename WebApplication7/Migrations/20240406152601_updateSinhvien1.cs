using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class updateSinhvien1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "SinhViens");

            migrationBuilder.AddColumn<string>(
                name: "Ten",
                table: "SinhViens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SinhViens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_UserId",
                table: "SinhViens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SinhViens_AspNetUsers_UserId",
                table: "SinhViens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SinhViens_AspNetUsers_UserId",
                table: "SinhViens");

            migrationBuilder.DropIndex(
                name: "IX_SinhViens_UserId",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "Ten",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SinhViens");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SinhViens",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
