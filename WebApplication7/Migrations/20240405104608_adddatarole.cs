using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class adddatarole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "508d2b81-cd4b-4d9f-9648-019b921e9e72", "1", "Admin", "Admin" },
                    { "ca0a2a13-552b-43d8-b519-3f52b527bffe", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "508d2b81-cd4b-4d9f-9648-019b921e9e72");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca0a2a13-552b-43d8-b519-3f52b527bffe");
        }
    }
}
