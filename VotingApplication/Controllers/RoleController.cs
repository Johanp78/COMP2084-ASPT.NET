using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VotingApplication.Data;
using VotingApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace VotingApplication.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public RoleController(ApplicationDbContext context, UserManager<User> userManager)
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

        // GET: Role
        public async Task<IActionResult> Index()
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            return View(await _context.Roles.ToListAsync());
        }

        // GET: Role/Details/5
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

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RolesId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RolesId,RolesName")] Role role)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Role/Edit/5
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

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            // Verify for role 1 (Admin role) to prevent editing
            if (role.RolesId == 1)
            {
                return Forbid(); // Forbid editing the admin role
            }

            return View(role);
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RolesId,RolesName")] Role role)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            if (id != role.RolesId)
            {
                return NotFound();
            }

            // Verify for role 1 (Admin role) to prevent editing
            if (role.RolesId == 1)
            {
                return Forbid(); // Forbid editing the admin role
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.RolesId))
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
            return View(role);
        }

        // GET: Role/Delete/5
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

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RolesId == id);
            if (role == null)
            {
                return NotFound();
            }

            // Verify for role 1 (Admin role) to prevent deletion
            if (role.RolesId == 1)
            {
                return Forbid(); // Forbid deleting the admin role
            }

            return View(role);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsUserAdmin())
            {
                return Forbid(); // Return 401 if user is not an Admin
            }

            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                // Verify for role 1 (Admin role) to prevent deletion
                if (role.RolesId == 1)
                {
                    return Forbid(); // Forbid deleting the admin role
                }

                _context.Roles.Remove(role);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.RolesId == id);
        }
    }
}
