using Microsoft.AspNetCore.Identity;

namespace BloggerBlazorServer.Data;

public static class SeedUsersRoles
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        // Seed Roles
        await EnsureRoleExists(roleManager, "Admin");
        await EnsureRoleExists(roleManager, "Contributor");
        await EnsureRoleExists(roleManager, "User");

        // Seed Users
        await EnsureUserExists(userManager, "a@a.a", "Med", "Hat", "Admin");
        await EnsureUserExists(userManager, "b@b.b", "Bruce", "Bond", "Contributor");
        await EnsureUserExists(userManager, "c@c.c", "Clark", "Kent", "User");
    }

    private static async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    private static async Task EnsureUserExists(
        UserManager<User> userManager,
        string email, 
        string firstName, 
        string lastName,
        string roleName)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await userManager.CreateAsync(user, "P@$$w0rd");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
