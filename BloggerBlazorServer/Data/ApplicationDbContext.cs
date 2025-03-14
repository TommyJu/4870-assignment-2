using BlogLibrary;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace BloggerBlazorServer.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
     public DbSet<Article> Article => Set<Article>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Article>().HasData(GetArticles());
        }
        public static IEnumerable<Article> GetArticles()
        {
            string[] p = { Directory.GetCurrentDirectory(), "wwwroot", "articles.csv"
};
            var csvFilePath = Path.Combine(p);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };
            var data = new List<Article>().AsEnumerable();
            using (var reader = new StreamReader(csvFilePath))
            {
                using (var csvReader = new CsvReader(reader, config))
                {
                    data = csvReader.GetRecords<Article>().ToList();
                }
            }
            return data;
        }
}
