using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BloggerBlazorServer.Services
{
    public class UserRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // get all the users and their roles
        public async Task<List<UserWithRoleDto>> GetAllUsersWithRolesAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserWithRoleDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserWithRoleDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }

            return userRoles;
        }

        // switch role between user and contributor (User <-> Contributor)
        public async Task<bool> ToggleUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("User"))
            {
                await _userManager.RemoveFromRoleAsync(user, "User");
                await _userManager.AddToRoleAsync(user, "Contributor");
            }
            else if (roles.Contains("Contributor"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Contributor");
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    // DTO (Data Transfer Object) for displaying user data
    public class UserWithRoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; } = "No Role";
    }
}
