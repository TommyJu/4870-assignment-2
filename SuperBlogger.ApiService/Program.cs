using Microsoft.EntityFrameworkCore;
using BloggerBlazorServer.Data;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add SQL Server connection
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__blogdb");
if (connectionString == null) {
    // no Aspire
    Console.WriteLine("Connection string 'blogdb' not found. Using default connection string.");
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (connectionString == null) {
        throw new InvalidOperationException("Connection string not found.");
    }
}
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(connectionString));



// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (exception != null)
        {
            Console.WriteLine($"Error: {exception.Error.Message}");
            Console.WriteLine($"Stack Trace: {exception.Error.StackTrace}");

            await context.Response.WriteAsJsonAsync(new
            {
                error = exception.Error.Message,
                stackTrace = exception.Error.StackTrace
            });
        }
    });
});

app.MapGet("/articles", async (ApplicationDbContext db) =>
{
    var articles = await db.Articles
        .Include(a => a.Contributor).ToListAsync();

    return articles;
});

app.MapGet("/users", async (ApplicationDbContext db) =>
{
    var users = await db.Users.ToListAsync();
    return users;
});

app.MapGet("/articles/{id}", async (ApplicationDbContext db, int id) =>
{
    var article = await db.Articles
    .Include(a => a.Contributor)
    .FirstOrDefaultAsync(a => a.ArticleId == id);
    return article is not null ? Results.Ok(article) : Results.NotFound();
});

app.MapDefaultEndpoints();

app.Run();
