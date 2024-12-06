using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VotingApplication.Data;
using VotingApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace VotingApplication.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Helper method to check if the current user is an Admin
        private bool IsUserAdmin()
        {
            var user = _userManager.GetUserAsync(User).Result; // Get the current user
            return user != null && user.UserRol == 1;  // Check if the user's role is 1 (Admin)
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role) // Include the relationship with Role if necessary
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Edit/5
        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Fetch roles and populate ViewBag
            var roles = await _context.Roles.ToListAsync(); // Fetch roles from the Roles DbSet
            ViewBag.Roles = new SelectList(roles, "RolesId", "RolesName"); // Use RolesId and RolesName

            // Fetch elections and populate ViewBag
            var elections = await _context.Elections
                .Where(e => e.ElectionStatus == 1) // Assuming you want only active elections
                .ToListAsync();

            ViewBag.Elections = new SelectList(elections, "ElectionId", "ElectionTitle"); // Use ElectionId and ElectionTitle

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, User user)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing user from the database
                    var existingUser = await _context.Users.FindAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Update properties manually (except ID)
                    existingUser.UserRol = user.UserRol;
                    existingUser.UserElection = user.UserElection;
                    existingUser.UserImage = user.UserImage;
                    existingUser.UserStatus = user.UserStatus;

                    // Update the entity state
                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirect after editing
            }

            // Fetch roles and elections again if ModelState is invalid
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "RolesId", "RolesName");
            ViewBag.Elections = new SelectList(await _context.Elections.Where(e => e.ElectionStatus == 1).ToListAsync(), "ElectionId", "ElectionTitle");

            return View(user);
        }


        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role) // Include the relationship with Role if necessary
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id && e.UserStatus != 3); // Check if user exists and is not soft-deleted
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(); // Return 404 if the user is not found
            }

            // Soft delete: Set UserStatus to 3 instead of removing the user
            user.UserStatus = 3;

            try
            {
                _context.Update(user); // Update the user entity
                await _context.SaveChangesAsync(); // Save changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == user.Id))
                {
                    return NotFound(); // User doesn't exist anymore
                }
                else
                {
                    throw; // Re-throw the exception if it's something else
                }
            }

            return RedirectToAction(nameof(Index)); // Redirect to Index after soft deletion
        }
    }
}
