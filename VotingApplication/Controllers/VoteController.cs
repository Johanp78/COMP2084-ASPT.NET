using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingApplication.Data;
using VotingApplication.Models;

namespace VotingApplication.Controllers
{
    [Authorize] // Ensures only logged-in users can vote
    public class VoteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display available elections for voting
        public async Task<IActionResult> Index()
        {
            var elections = await _context.Elections
                .Include(e => e.Candidates)
                .Where(e => e.ElectionStatus == 1) // Only active elections
                .ToListAsync();
            return View(elections);
        }

        // Show the voting page for a specific election
        public async Task<IActionResult> Vote(int id)
        {
            var election = await _context.Elections
                .Include(e => e.Candidates)
                .FirstOrDefaultAsync(e => e.ElectionId == id);

            if (election == null || election.ElectionStatus != 1)
            {
                return NotFound();
            }

            return View(election);
        }

        // Process a user's vote
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitVote(int electionId, int candidateId)
        {
            // Ensure the user hasn't already voted
            var userId = User.Identity?.Name; // Update this based on your UserId setup
            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.VotesElection == electionId && v.Candidate.CandidateId == candidateId);

            if (existingVote != null)
            {
                return BadRequest("You have already voted in this election.");
            }

            // Record the vote
            var vote = new Vote
            {
                VotesElection = electionId,
                VotesCandidate = candidateId,
                VotesDatetime = DateTime.Now
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
