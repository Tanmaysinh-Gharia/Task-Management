using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$YHcJvjxbP78hEJROwJ4VeOQ51M4LHFbqK9p8cW/HqD66lUlzEVSRi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$2EO8SJK1eZy6KBLz3h9OuOzskDcoOjsQv5H80YYbRtjUIMiYO3N4C");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$sO6ten5PPZk6CQOZd0cYDuYZliSSeo/rzOBdS.xi4NZoC5ckWMEEG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$wGGAWtgAt6r.m3DnOYCcdON4J65OZ8r/7NTDVl/srY6AXo4yXADOi");
        }
    }
}
