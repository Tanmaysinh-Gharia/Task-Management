using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TaskDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "ChangeTime",
                value: new DateTime(2025, 5, 4, 1, 1, 1, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DueDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 4, 1, 1, 1, 0, DateTimeKind.Local), new DateTime(2025, 5, 10, 1, 1, 1, 0, DateTimeKind.Local), new DateTime(2025, 5, 5, 1, 1, 1, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 5, 4, 1, 1, 1, 0, DateTimeKind.Local), "$2a$11$B8f2QyJ9.8pNcR2E7IGMpeezHjLkdRfhLBVNRhqm0jPF6FbfKpx7G" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 5, 3, 1, 1, 1, 0, DateTimeKind.Local), "$2a$11$ffUcKzIE10PMT6MFkdotY.x9GJCr80u4pqPLiY9IlLQQ4HqaRIr16" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TaskDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "ChangeTime",
                value: new DateTime(2025, 5, 4, 8, 10, 50, 58, DateTimeKind.Local).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DueDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 4, 8, 10, 50, 57, DateTimeKind.Local).AddTicks(6703), new DateTime(2025, 5, 11, 8, 10, 50, 57, DateTimeKind.Local).AddTicks(6179), new DateTime(2025, 5, 4, 8, 10, 50, 57, DateTimeKind.Local).AddTicks(6927) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 5, 4, 8, 10, 49, 820, DateTimeKind.Local).AddTicks(110), "$2a$11$lt7sZt.QSjHnoJH/l8Oe6Opq.wLrAiOggmm6YcPhJxeKCwgxarvdK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 5, 4, 8, 10, 50, 55, DateTimeKind.Local).AddTicks(8944), "$2a$11$xsxPgQdVRvLnppxZ.ap2NOCqH7eSHAi.DVvrInhMy/MnymayiMjKm" });
        }
    }
}
