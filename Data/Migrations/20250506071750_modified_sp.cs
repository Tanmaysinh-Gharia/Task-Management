using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class modified_sp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var spPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Migrations", "Scripts", "StoredProcedures");

            if (Directory.Exists(spPath))
            {
                foreach (var file in Directory.GetFiles(spPath, "*.sql"))
                {
                    var sql = File.ReadAllText(file);
                    migrationBuilder.Sql(sql);
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFilteredTasks");
        }
    }
}
