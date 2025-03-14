using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BloggerBlazorServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class M1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.ArticleId);
                });

            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "ArticleId", "Body", "CreateDate", "EndDate", "StartDate", "Title", "UserName" },
                values: new object[,]
                {
                    { 1, "There is a lot of hype around the new creatures known as doughcats in riot's top tier autobattle simulator, TFT.", new DateTime(2025, 3, 7, 14, 30, 45, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 21, 14, 30, 45, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 7, 14, 30, 45, 0, DateTimeKind.Unspecified), "How Dough Cats are taking over the World", "a@a.a" },
                    { 2, "Artificial Intelligence is transforming how we interact with technology, from chatbots to personal assistants.", new DateTime(2025, 3, 8, 10, 15, 30, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 22, 10, 15, 30, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 8, 10, 15, 30, 0, DateTimeKind.Unspecified), "The Rise of AI Companions", "b@b.b" },
                    { 3, "Scientists are uncovering new marine species and mysteries hidden in the unexplored depths of the ocean.", new DateTime(2025, 3, 9, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 23, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 9, 9, 0, 0, 0, DateTimeKind.Unspecified), "Exploring the Deep Ocean", "c@c.c" },
                    { 4, "Space agencies and private companies are racing to establish a human presence on Mars within the next decade.", new DateTime(2025, 3, 10, 12, 45, 20, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 24, 12, 45, 20, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 10, 12, 45, 20, 0, DateTimeKind.Unspecified), "Mars Colonization: The Next Step", "d@d.d" },
                    { 5, "Researchers have made significant advancements in quantum computing, paving the way for solving complex problems.", new DateTime(2025, 3, 11, 16, 20, 10, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 25, 16, 20, 10, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 11, 16, 20, 10, 0, DateTimeKind.Unspecified), "Quantum Computing Breakthroughs", "e@e.e" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Article");
        }
    }
}
