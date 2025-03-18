using BlogLibrary;
using Microsoft.AspNetCore.Identity;

namespace BloggerBlazorServer.Data;

public static class SeedArticles
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Seed Articles
        if (!context.Articles.Any())
        {
            var articles = new List<Article>
            {
                new Article
                {
                    Title = "How Dough Cats are Taking Over the World Twice",
                    Body = "There is a lot of hype around the new creatures known as doughcats in Riot's top-tier autobattle simulator, TFT.",
                    CreateDate = new DateTime(2025, 3, 7, 14, 30, 45),
                    StartDate = new DateTime(2025, 3, 7, 14, 30, 45),
                    EndDate = new DateTime(2025, 3, 21, 14, 30, 45),
                    ContributorId = await GetUserIdByEmail(userManager, "a@a.a")
                },
                new Article
                {
                    Title = "The Rise of AI Companions",
                    Body = "Artificial Intelligence is transforming how we interact with technology, from chatbots to personal assistants.",
                    CreateDate = new DateTime(2025, 3, 8, 10, 15, 30),
                    StartDate = new DateTime(2025, 3, 8, 10, 15, 30),
                    EndDate = new DateTime(2025, 3, 22, 10, 15, 30),
                    ContributorId = await GetUserIdByEmail(userManager, "b@b.b")
                },
                new Article
                {
                    Title = "Exploring the Deep Ocean",
                    Body = "Scientists are uncovering new marine species and mysteries hidden in the unexplored depths of the ocean.",
                    CreateDate = new DateTime(2025, 3, 9, 9, 0, 0),
                    StartDate = new DateTime(2025, 3, 9, 9, 0, 0),
                    EndDate = new DateTime(2025, 3, 23, 9, 0, 0),
                    ContributorId = await GetUserIdByEmail(userManager, "c@c.c")
                },
                new Article
                {
                    Title = "Mars Colonization: The Next Step",
                    Body = "Space agencies and private companies are racing to establish a human presence on Mars within the next decade.",
                    CreateDate = new DateTime(2025, 3, 10, 12, 45, 20),
                    StartDate = new DateTime(2025, 3, 10, 12, 45, 20),
                    EndDate = new DateTime(2025, 3, 24, 12, 45, 20),
                    ContributorId = await GetUserIdByEmail(userManager, "a@a.a")
                },
                new Article
                {
                    Title = "Quantum Computing Breakthroughs",
                    Body = "Researchers have made significant advancements in quantum computing, paving the way for solving complex problems.",
                    CreateDate = new DateTime(2025, 3, 11, 16, 20, 10),
                    StartDate = new DateTime(2025, 3, 11, 16, 20, 10),
                    EndDate = new DateTime(2025, 3, 25, 16, 20, 10),
                    ContributorId = await GetUserIdByEmail(userManager, "b@b.b")
                }
            };

            await context.Articles.AddRangeAsync(articles);
            await context.SaveChangesAsync();
        }
    }

    // Helper Method to Retrieve User Id by Email
    private static async Task<string?> GetUserIdByEmail(UserManager<User> userManager, string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user?.Id;
    }
}
