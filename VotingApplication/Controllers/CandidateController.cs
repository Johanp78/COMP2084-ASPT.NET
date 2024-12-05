using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VotingApplication.Data;
using Microsoft.AspNetCore.Identity;
using VotingApplication.Models;

namespace VotingApplication.Controllers
{
    [Authorize] // This ensures that only authenticated users can access the controller
    public class CandidateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        // Constructor
        public CandidateController(ApplicationDbContext context, UserManager<User> userManager)
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

        // GET: Candidate
        public async Task<IActionResult> Index()
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            var applicationDbContext = _context.Candidates.Include(c => c.Election).Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Candidate/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .Include(c => c.Election)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // GET: Candidate/Create
        public IActionResult Create()
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            ViewData["ElectionElection"] = new SelectList(_context.Elections, "ElectionId", "ElectionTitle");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName"); // UserName
            return View();
        }

        // POST: Candidate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateId,UserId,ElectionId")] Candidate candidate)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (candidate.ElectionId == 0)
            {
                ModelState.AddModelError("ElectionId", "Election ID is required.");
            }

            if (ModelState.IsValid)
            {
                _context.Candidates.Add(candidate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ElectionElection"] = new SelectList(_context.Elections, "ElectionId", "ElectionTitle", candidate.ElectionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", candidate.UserId); // UserName
            return View(candidate);
        }

        // GET: Candidate/Edit/5
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

            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }

            ViewData["ElectionElection"] = new SelectList(_context.Elections, "ElectionId", "ElectionTitle", candidate.ElectionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", candidate.UserId); // Mostrar UserName
            return View(candidate);
        }

        // POST: Candidate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidateId,UserId,ElectionId")] Candidate candidate)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (id != candidate.CandidateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateExists(candidate.CandidateId))
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

            ViewData["ElectionElection"] = new SelectList(_context.Elections, "ElectionId", "ElectionTitle", candidate.ElectionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", candidate.UserId); // Mostrar UserName
            return View(candidate);
        }

        // GET: Candidate/Delete/5
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

            var candidate = await _context.Candidates
                .Include(c => c.Election)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // POST: Candidate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a candidate exists
        private bool CandidateExists(int id)
        {
            return _context.Candidates.Any(e => e.CandidateId == id);
        }
    }
}
