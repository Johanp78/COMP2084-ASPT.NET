using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingApplication.Models;
using VotingApplication.Data;

namespace VotingApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Fetch only active elections (status 1) and allow anonymous access
        public async Task<IActionResult> Index()
        {
            var activeElections = await _context.Elections
                                                 .Where(e => e.ElectionStatus == 1) // Only active elections
                                                 .ToListAsync();

            // Get the current user's role (UserRol)
            var currentUser = await _userManager.GetUserAsync(User);
            var isAdmin = currentUser != null && currentUser.UserRol == 1;

            // Pass the data and role status to the view
            ViewData["IsAdmin"] = isAdmin;
            ViewData["CurrentUser"] = currentUser?.UserName; // Pass the user's name if needed

            return View(activeElections); // Return the list of active elections to the view
        }

        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && currentUser.UserRol == 1)
            {
                return View();
            }
            return Unauthorized();
        }

        // Other actions...
    }
}
