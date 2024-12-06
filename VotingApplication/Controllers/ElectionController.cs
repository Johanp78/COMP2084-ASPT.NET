using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VotingApplication.Data;
using VotingApplication.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization; // Ensure this namespace is included for logging
using Microsoft.AspNetCore.Identity;

namespace VotingApplication.Controllers
{
    [Authorize] // Ensures that only authenticated users can access the controller
    public class ElectionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ElectionController> _logger;
        private readonly UserManager<User> _userManager;

        // Constructor with logger injection
        public ElectionController(ApplicationDbContext context, ILogger<ElectionController> logger, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // Helper method to check if the current user is an Admin
        private bool IsUserAdmin()
        {
            var user = _userManager.GetUserAsync(User).Result; // Get the current user
            return user != null && user.UserRol == 1;  // Check if the user's role is 1 (Admin)
        }

        // GET: Election
        public async Task<IActionResult> Index()
        {
            // Allow all users to view the list of elections
            var elections = await _context.Elections.ToListAsync();
            return View(elections);
        }


        // GET: Election/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var election = await _context.Elections
                .Include(e => e.Candidates) // Load candidates
                .Include(e => e.Votes)      // Load votes
                .FirstOrDefaultAsync(m => m.ElectionId == id);

            if (election == null)
            {
                return NotFound();
            }

            return View(election);
        }


        // POST: Election/CastVote
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CastVote(int electionId, int candidateId)
        {
            if (electionId <= 0 || candidateId <= 0)
            {
                return BadRequest("Invalid election or candidate.");
            }

            var vote = new Vote
            {
                VotesElection = electionId,
                VotesCandidate = candidateId,
                VotesDatetime = DateTime.Now
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            // Get the current user
            var userId = User.Identity.Name; // This gets the username or you can use User.FindFirstValue(ClaimTypes.NameIdentifier)
            var user = await _userManager.FindByNameAsync(userId); // Use _userManager to find the user by username

            if (user != null)
            {
                // Reset the UserElection to 1 after casting the vote
                user.UserElection = 1;
                await _userManager.UpdateAsync(user); // Use _userManager to update the user
            }

            return RedirectToAction("Index", "Home");
        }


        // GET: Election/Create
        public IActionResult Create()
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            return View();
        }

        // POST: Election/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElectionTitle,ElectionStatus,ElectionStartDate,ElectionEndDate")] Election election)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            _logger.LogInformation("POST WAS TRIGGERED");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
                return View(election);
            }

            _context.Add(election);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Election/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (id == null)
            {
                return NotFound();
            }

            var election = await _context.Elections.FindAsync(id);
            if (election == null)
            {
                return NotFound();
            }
            return View(election);
        }

        // POST: Election/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ElectionId,ElectionTitle,ElectionStatus,ElectionStartDate,ElectionEndDate")] Election election)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (id != election.ElectionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(election);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectionExists(election.ElectionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(election);
        }

        // GET: Election/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (id == null)
            {
                return NotFound();
            }

            var election = await _context.Elections
                .FirstOrDefaultAsync(m => m.ElectionId == id);
            if (election == null)
            {
                return NotFound();
            }

            return View(election);
        }

        // POST: Election/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            var election = await _context.Elections.FindAsync(id);
            if (election != null)
            {
                _context.Elections.Remove(election);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if an election exists
        private bool ElectionExists(int id)
        {
            return _context.Elections.Any(e => e.ElectionId == id);
        }
    }
}
