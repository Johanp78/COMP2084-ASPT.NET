using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VotingApplication.Data;
using VotingApplication.Models;

namespace VotingApplication.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Candidate
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Candidates.Include(c => c.Election).Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Candidate/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
            ViewData["ElectionElection"] = new SelectList(_context.Elections, "ElectionId", "ElectionTitle");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName"); // UserName
            return View();
        }

        // POST: Candidate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateId,UserId,ElectionId")] Candidate candidate)
        {

            if (candidate.ElectionId == 0)
            {
                ModelState.AddModelError("ElectionId", "Election ID is required.");
            }

            if (ModelState.IsValid)
            {
                _context.Candidates.Add(candidate);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ElectionElection"] = new SelectList(_context.Elections, "ElectionId", "ElectionTitle", candidate.ElectionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", candidate.UserId); // UserName
            return View(candidate);
        }

        // GET: Candidate/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateExists(int id)
        {
            return _context.Candidates.Any(e => e.CandidateId == id);
        }
    }
}
