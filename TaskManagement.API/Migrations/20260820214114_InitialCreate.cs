using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AssignedTo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AssignedTo", "CreatedDate", "Description", "ModifiedDate", "Priority", "Status", "Title" },
                values: new object[,]
                {
                    { 1, "Priya Sharma", new DateTime(2026, 6, 10, 9, 0, 0, 0, DateTimeKind.Utc), "Configure GitHub Actions for build and test automation on every pull request.", new DateTime(2026, 6, 18, 14, 30, 0, 0, DateTimeKind.Utc), "High", "Done", "Set up CI/CD pipeline" },
                    { 2, "Marcus Chen", new DateTime(2026, 6, 12, 10, 15, 0, 0, DateTimeKind.Utc), "Model the core entities and relationships for the task management system.", new DateTime(2026, 6, 20, 16, 0, 0, 0, DateTimeKind.Utc), "Critical", "Done", "Design database schema" },
                    { 3, "Marcus Chen", new DateTime(2026, 7, 1, 8, 45, 0, 0, DateTimeKind.Utc), "Add JWT-based authentication to protect API endpoints.", new DateTime(2026, 8, 5, 11, 20, 0, 0, DateTimeKind.Utc), "High", "InProgress", "Implement authentication middleware" },
                    { 4, "Priya Sharma", new DateTime(2026, 7, 3, 9, 30, 0, 0, DateTimeKind.Utc), "Support filtering tasks by status and priority via query parameters.", new DateTime(2026, 7, 15, 13, 0, 0, 0, DateTimeKind.Utc), "Medium", "Done", "Build task filtering API" },
                    { 5, "Elena Rodriguez", new DateTime(2026, 7, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Display tasks in a table with sortable columns.", new DateTime(2026, 8, 10, 15, 45, 0, 0, DateTimeKind.Utc), "Medium", "InProgress", "Create React task list UI" },
                    { 6, "Sam O'Neill", new DateTime(2026, 7, 20, 9, 0, 0, 0, DateTimeKind.Utc), "Cover filtering, sorting, and soft delete logic.", new DateTime(2026, 7, 20, 9, 0, 0, 0, DateTimeKind.Utc), "Medium", "ToDo", "Write unit tests for TaskService" },
                    { 7, "Marcus Chen", new DateTime(2026, 7, 22, 11, 15, 0, 0, DateTimeKind.Utc), "Return consistent error responses across all endpoints.", new DateTime(2026, 8, 1, 9, 30, 0, 0, DateTimeKind.Utc), "High", "Done", "Add global exception handling" },
                    { 8, "Elena Rodriguez", new DateTime(2026, 8, 2, 13, 40, 0, 0, DateTimeKind.Utc), "Priority badges do not match the design spec on the task cards.", new DateTime(2026, 8, 2, 13, 40, 0, 0, DateTimeKind.Utc), "Low", "ToDo", "Fix priority badge colors" },
                    { 9, "Priya Sharma", new DateTime(2026, 8, 5, 8, 0, 0, 0, DateTimeKind.Utc), "Investigate slow response times on the task summary endpoint under load.", new DateTime(2026, 8, 15, 17, 10, 0, 0, DateTimeKind.Utc), "Critical", "InProgress", "Optimize summary query performance" },
                    { 10, "Sam O'Neill", new DateTime(2026, 8, 8, 10, 30, 0, 0, DateTimeKind.Utc), "Bundle the API, frontend, and SQL Server into a single Compose file.", new DateTime(2026, 8, 8, 10, 30, 0, 0, DateTimeKind.Utc), "Medium", "ToDo", "Set up Docker Compose for local dev" },
                    { 11, "Elena Rodriguez", new DateTime(2026, 8, 12, 9, 20, 0, 0, DateTimeKind.Utc), "Ensure Swagger annotations accurately describe all endpoints.", new DateTime(2026, 8, 12, 9, 20, 0, 0, DateTimeKind.Utc), "Low", "ToDo", "Review API documentation" },
                    { 12, "Marcus Chen", new DateTime(2026, 8, 14, 14, 0, 0, 0, DateTimeKind.Utc), "Gather feedback on the last two-week sprint and identify improvements.", new DateTime(2026, 8, 16, 10, 5, 0, 0, DateTimeKind.Utc), "Low", "Done", "Plan sprint retrospective" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Priority",
                table: "Tasks",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Status",
                table: "Tasks",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
