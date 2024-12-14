using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Store.Controllers
{
    [Authorize(Roles = "Administrator")] 
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Utilizatorul nu a fost găsit.");
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest("Rolul specificat nu există.");
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                return Ok($"Utilizatorul {user.UserName} a fost adăugat în rolul {role}.");
            }

            return BadRequest(result.Errors);
        }

        

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser()
        {
            var user = new IdentityUser
            {
                UserName = "testuser@example.com",
                Email = "testuser@example.com",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, "Password123!");
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Administrator");
                if (roleResult.Succeeded)
                {
                    return Ok($"Utilizatorul {user.UserName} a fost creat și adăugat în rolul Administrator.");
                }
                else
                {
                    return BadRequest(roleResult.Errors);
                }
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        
    }
}


