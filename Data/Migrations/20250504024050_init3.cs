using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TaskDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "ChangeTime",
                value: new DateTime(2025, 5, 4, 0, 32, 15, 413, DateTimeKind.Local).AddTicks(1268));

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DueDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 4, 0, 32, 15, 412, DateTimeKind.Local).AddTicks(7414), new DateTime(2025, 5, 11, 0, 32, 15, 412, DateTimeKind.Local).AddTicks(6467), new DateTime(2025, 5, 4, 0, 32, 15, 412, DateTimeKind.Local).AddTicks(7923) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 5, 4, 0, 32, 15, 165, DateTimeKind.Local).AddTicks(6126), "$2a$11$A/WAuusYrNH.44VRkkiGQuRS7KBm1rIZz/E94ZB2VDCEpQokh2evy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 5, 4, 0, 32, 15, 411, DateTimeKind.Local).AddTicks(3364), "$2a$11$QY19Ergj5AX/yA3EGw9WJOQ2pxw0436jeOPa9j4KVKKB8nEhPrqNW" });
        }
    }
}
