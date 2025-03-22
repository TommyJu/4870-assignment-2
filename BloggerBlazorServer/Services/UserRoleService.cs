using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BloggerBlazorServer.Services
{
    public class UserRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRoleService(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        // get current logged in user
        public async Task<UserWithRoleDto?> GetCurrentUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity!.IsAuthenticated)
            {
                return null;
            }

            var currentUser = await _userManager.GetUserAsync(user);

            if (currentUser == null) return null;

            var roles = await _userManager.GetRolesAsync(currentUser);

            return new UserWithRoleDto
            {
                Id = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email,
                Role = roles.FirstOrDefault() ?? "No Role"
            };
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

        // delete user by Email
        public async Task<bool> DeleteUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false; // cannot find specific user
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
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
