using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BloggerBlazorServer.Components;
using BloggerBlazorServer.Components.Account;
using BloggerBlazorServer.Data;
using BlogLibrary;
using Aspire;

var builder = WebApplication.CreateBuilder(args);

//builder.AddSqlServerDbContext<ApplicationDbContext>("blogdb");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(connectionString));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.AddSqlServerDbContext<ApplicationDbContext>("blogdb");

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__blogdb");
if (connectionString == null) {
    // no Aspire
    Console.WriteLine("Connection string 'blogdb' not found. Using default connection string.");
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (connectionString == null) {
        throw new InvalidOperationException("Connection string not found.");
    }
}

Console.WriteLine($"Connection string: {connectionString}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>(
options => {
    options.Stores.MaxLengthForKeys = 128;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddRoles<IdentityRole>();

builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

//Register Seeder
builder.Services.AddScoped<DbSeeder>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try{

    }
    catch (Exception ex) {
        Console.WriteLine(ex);
    }
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    await DbSeeder.SeedDataAsync(services);
}

app.Run();
