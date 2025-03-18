using BlogLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BloggerBlazorServer.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }
     public DbSet<Article> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        DbSeeder dbSeeder = new();
foreach (var article in dbSeeder.Articles)
{
    Console.WriteLine(article.Title);
}

        builder.Entity<IdentityRole>().HasData(dbSeeder.Roles);
        builder.Entity<User>().HasData(dbSeeder.Users);
        builder.Entity<Article>().HasData(dbSeeder.Articles);
    }
    
}

// using BlogLibrary;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;
// using System.Globalization;
// using System.Text;
// using CsvHelper;
// using CsvHelper.Configuration;

// namespace BloggerBlazorServer.Data;

// public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
// {
//      public DbSet<Article> Articles => Set<Article>();
//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);
//             modelBuilder.Entity<Article>()
//             .HasOne(a => a.Contributor)  //Each article has one user as a contributor
//             .WithMany()  //User is associated with many articles
//             .OnDelete(DeleteBehavior.Cascade); //Deletes Article if User is deleted
//         }
// }
