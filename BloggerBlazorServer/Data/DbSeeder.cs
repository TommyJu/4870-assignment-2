using System;
using BlogLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BloggerBlazorServer.Data;

public class DbSeeder
{

    //Collection of Articles
    private readonly List<Article> _articles;
    private readonly List<User> _users;
    private readonly List<IdentityRole> _roles;

    //Constructor for DbSeeder
    public DbSeeder() {
        _articles = GetArticles();
        _users = GetUsers();
        _roles = GetRoles();
    }

    //Getters
    public List<Article> Articles { get {return _articles;}}
    public List<User> Users { get { return _users;}}
    public List<IdentityRole> Roles { get {return _roles;}}
    
    //Seeding List Functions
    private List<User> GetUsers() {
        
        string password = "P@$$w0rd";
        var passwordHasher = new PasswordHasher<User>();

        //Admin User Preset Fields
        var adminUser = new User {
            UserName = "a@a.a",
            Email = "a@a.a",
            EmailConfirmed = true,
            FirstName = "Med",
            LastName = "Hat"
        };
        adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
        adminUser.NormalizedEmail = adminUser.Email.ToUpper();

        //Password Hashing
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, password);

        //Contributor User Preset Fields
        var contributorUser = new User {
            UserName = "c@c.c",
            Email = "c@c.c",
            EmailConfirmed = true,
            FirstName = "Bruce",
            LastName = "Link",
        };
        contributorUser.NormalizedUserName = contributorUser.UserName.ToUpper();
        contributorUser.NormalizedEmail = contributorUser.Email.ToUpper();
        contributorUser.PasswordHash = passwordHasher.HashPassword(contributorUser, password);

        List<User> users = new List<User> {
            adminUser,
            contributorUser
        };

        return users;
    }
    
    private List<Article> GetArticles() {
        var articles = new List<Article>
            {
                new Article
            {
                ArticleId = 1,
                Title = "How Dough Cats are Taking Over the World Twice",
                Body = "There is a lot of hype around the new creatures known as doughcats in Riot's top-tier autobattle simulator, TFT.",
                CreateDate = new DateTime(2025, 3, 7, 14, 30, 45),
                StartDate = new DateTime(2025, 3, 7, 14, 30, 45),
                EndDate = new DateTime(2025, 3, 21, 14, 30, 45),
                UserName = "a@a.a"
            },
                new Article
            {
                ArticleId = 2,
                Title = "The Rise of AI Companions",
                Body = "Artificial Intelligence is transforming how we interact with technology, from chatbots to personal assistants.",
                CreateDate = new DateTime(2025, 3, 8, 10, 15, 30),
                StartDate = new DateTime(2025, 3, 8, 10, 15, 30),
                EndDate = new DateTime(2025, 3, 22, 10, 15, 30),
                UserName = "c@c.c"
            },
            new Article
            {
                ArticleId = 3,
                Title = "Exploring the Deep Ocean",
                Body = "Scientists are uncovering new marine species and mysteries hidden in the unexplored depths of the ocean.",
                CreateDate = new DateTime(2025, 3, 9, 9, 0, 0),
                StartDate = new DateTime(2025, 3, 9, 9, 0, 0),
                EndDate = new DateTime(2025, 3, 23, 9, 0, 0),
                UserName = "a@a.a"
            },
            new Article
            {
                ArticleId = 4,
                Title = "Mars Colonization: The Next Step",
                Body = "Space agencies and private companies are racing to establish a human presence on Mars within the next decade.",
                CreateDate = new DateTime(2025, 3, 10, 12, 45, 20),
                StartDate = new DateTime(2025, 3, 10, 12, 45, 20),
                EndDate = new DateTime(2025, 3, 24, 12, 45, 20),
                UserName = "c@c.c"
            },
            new Article
            {
                ArticleId = 5,
                Title = "Quantum Computing Breakthroughs",
                Body = "Researchers have made significant advancements in quantum computing, paving the way for solving complex problems.",
                CreateDate = new DateTime(2025, 3, 11, 16, 20, 10),
                StartDate = new DateTime(2025, 3, 11, 16, 20, 10),
                EndDate = new DateTime(2025, 3, 25, 16, 20, 10),
                UserName = "a@a.a"
            }
        };

        return articles;
    }

    private List<IdentityRole> GetRoles() {

        //Admin Role
        var adminRole = new IdentityRole("Admin") { Id = "admin" };
        adminRole.NormalizedName = adminRole.Name!.ToUpper();
        
        //Contributor Role
        var contributorRole = new IdentityRole("Contributor") { Id = "contributor" };
        contributorRole.NormalizedName = contributorRole.Name!.ToUpper();

        List<IdentityRole> roles = new List<IdentityRole>() {
            adminRole,
            contributorRole,
        };

        return roles;
    }

    //Method for linking data (article contributors and role setting)
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        //Retrieves Application Database Context
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        //Retrieves UserManager services
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        //Retrieves RoleManager services
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //Seeding Roles Section
        string[] roleNames = { "Admin", "Contributor" };
        foreach (var roleName in roleNames)
        {
            //Check if each roleName exists, and if it doesn't, then create it
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        //Assign Admin Role
        var adminUser = await userManager.FindByEmailAsync("a@a.a");
        if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        //Assign Contributor Role
        var contributorUser = await userManager.FindByEmailAsync("c@c.c");
        if (contributorUser != null && !await userManager.IsInRoleAsync(contributorUser, "Contributor"))
        {
            await userManager.AddToRoleAsync(contributorUser, "Contributor");
        }


        //Seed Article Contributors
        var articles = await context.Articles.ToListAsync();
        var count = 0;

            // Loop through each article and add the contributor
        foreach (var article in articles)
        {
            count++;
            if (count % 2 == 0) {
                // Fetch the contributor (User) based on UserName (or any other relevant field)
                article.Contributor = adminUser;
            } else {
                article.Contributor = contributorUser;
            }
            context.Articles.Update(article);
        }
    }
}

// using System;
// using BlogLibrary;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;

// namespace BloggerBlazorServer.Data;

// public class DbSeeder
// {
    
//     //Method for handling seeding
//     public static async Task SeedDataAsync(IServiceProvider serviceProvider)
//     {
//         // Retrieves Application Database Context
//         var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

//         //Retrieves UserManager services
//         var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

//         //Retrieves RoleManager services
//         var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//         //Seeding Roles Section
//         string[] roleNames = { "Admin", "Contributor" };
//         foreach (var roleName in roleNames)
//         {
//             //Check if each roleName exists, and if it doesn't, then create it
//             if (!await roleManager.RoleExistsAsync(roleName))
//             {
//                 await roleManager.CreateAsync(new IdentityRole(roleName));
//             }
//         }

//         //Seed Users
//         string password = "P@$$w0rd";

//         var adminUser = await userManager.FindByEmailAsync("a@a.a");
//         if (adminUser == null)
//         {
//             adminUser = new User
//             {
//                 UserName = "a@a.a",
//                 Email = "a@a.a",
//                 EmailConfirmed = true,
//                 FirstName = "Admin",
//                 LastName = "User"
//             };
//             //Inserting Normalized capital fields
//             adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
//             adminUser.NormalizedEmail = adminUser.Email.ToUpper();

//             //Automatically hashes the password when storing it with CreateAsync method
//             var result = await userManager.CreateAsync(adminUser, password);

//             //Error Logging if result fails to create user
//             if (!result.Succeeded)
//             {
//                 foreach (var error in result.Errors)
//                 {
//                    Console.WriteLine(error.Description); 
//                 }
//             } else {
//                 //Assigns adminUser the role of Admin
//                 await userManager.AddToRoleAsync(adminUser, "Admin");
//             }
//         }


//         var contributorUser = await userManager.FindByEmailAsync("c@c.c");
//         if (contributorUser == null)
//         {
//             contributorUser = new User {
//                 UserName = "c@c.c",
//                 Email = "c@c.c",
//                 EmailConfirmed = true,
//                 FirstName = "Bruce",
//                 LastName = "Link",
//             };
//             contributorUser.NormalizedUserName = contributorUser.UserName.ToUpper();
//             contributorUser.NormalizedEmail = contributorUser.Email.ToUpper();

//             //Automatically hashes the password when storing it with CreateAsync method
//             var result = await userManager.CreateAsync(contributorUser, password);

//             //Error Logging if result fails to create user
//             if (!result.Succeeded)
//             {
//                 foreach (var error in result.Errors)
//                 {
//                    Console.WriteLine(error.Description); 
//                 }
//             } else {
//                 //Assigns contributor user the role of Contributor
//                 await userManager.AddToRoleAsync(contributorUser, "Contributor");
//             }
//         }

//         //Seed Articles

//         //Ensures table is not null
//         if(context.Articles is not null)
//         {
//             var articles = new List<Article>
//             {
//                 new Article
//             {
//                 ArticleId = 1,
//                 Title = "How Dough Cats are Taking Over the World Twice",
//                 Body = "There is a lot of hype around the new creatures known as doughcats in Riot's top-tier autobattle simulator, TFT.",
//                 CreateDate = new DateTime(2025, 3, 7, 14, 30, 45),
//                 StartDate = new DateTime(2025, 3, 7, 14, 30, 45),
//                 EndDate = new DateTime(2025, 3, 21, 14, 30, 45),
//                 UserName = "a@a.a",
//                 Contributor = await userManager.FindByEmailAsync("a@a.a")
//             },
//             new Article
//             {
//                 ArticleId = 2,
//                 Title = "The Rise of AI Companions",
//                 Body = "Artificial Intelligence is transforming how we interact with technology, from chatbots to personal assistants.",
//                 CreateDate = new DateTime(2025, 3, 8, 10, 15, 30),
//                 StartDate = new DateTime(2025, 3, 8, 10, 15, 30),
//                 EndDate = new DateTime(2025, 3, 22, 10, 15, 30),
//                 UserName = "b@b.b",
//                 Contributor = await userManager.FindByEmailAsync("a@a.a")
//             },
//             new Article
//             {
//                 ArticleId = 3,
//                 Title = "Exploring the Deep Ocean",
//                 Body = "Scientists are uncovering new marine species and mysteries hidden in the unexplored depths of the ocean.",
//                 CreateDate = new DateTime(2025, 3, 9, 9, 0, 0),
//                 StartDate = new DateTime(2025, 3, 9, 9, 0, 0),
//                 EndDate = new DateTime(2025, 3, 23, 9, 0, 0),
//                 UserName = "c@c.c",
//                 Contributor = await userManager.FindByEmailAsync("a@a.a")
//             },
//             new Article
//             {
//                 ArticleId = 4,
//                 Title = "Mars Colonization: The Next Step",
//                 Body = "Space agencies and private companies are racing to establish a human presence on Mars within the next decade.",
//                 CreateDate = new DateTime(2025, 3, 10, 12, 45, 20),
//                 StartDate = new DateTime(2025, 3, 10, 12, 45, 20),
//                 EndDate = new DateTime(2025, 3, 24, 12, 45, 20),
//                 UserName = "d@d.d",
//                 Contributor = await userManager.FindByEmailAsync("c@c.c")
//             },
//             new Article
//             {
//                 ArticleId = 5,
//                 Title = "Quantum Computing Breakthroughs",
//                 Body = "Researchers have made significant advancements in quantum computing, paving the way for solving complex problems.",
//                 CreateDate = new DateTime(2025, 3, 11, 16, 20, 10),
//                 StartDate = new DateTime(2025, 3, 11, 16, 20, 10),
//                 EndDate = new DateTime(2025, 3, 25, 16, 20, 10),
//                 UserName = "e@e.e",
//                 Contributor = await userManager.FindByEmailAsync("c@c.c")
//             }
//         };

//         Console.WriteLine(articles);

//         //Loop through each seed article to check if they exist in the database
//         foreach (var article in articles)
//         {
//             bool exists = await context.Articles.AnyAsync(a => a.Title == article.Title);

//             //If it doesn't not exist in the database, add into Article database
//             if (!exists)
//             {
//                 var contributor = await userManager.FindByEmailAsync(article.UserName!);
        
//                 if (contributor != null)
//                 {
//                     context.Attach(contributor); // Attach the contributor to the context
//                     article.Contributor = contributor;
//                     await context.Articles.AddAsync(article);
//                 }
//                 else
//                 {
//                     Console.WriteLine($"Contributor with email {article.UserName} not found.");
//                 }
//             }

//         }

//         //Save changes to database context
//         await context.SaveChangesAsync();
//         }
//     }
// }