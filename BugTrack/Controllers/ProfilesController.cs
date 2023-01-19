using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrack.Data;
using BugTrack.Models;
using Microsoft.AspNetCore.Identity;
using BugTrack.Areas.Identity.Data;
using BugTrack.ViewModels.VMProfiles;

namespace BugTrack.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<BugUser> _userManager;

        public ProfilesController(ApplicationDbContext context, UserManager<BugUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Profiles
        public async Task<IActionResult> Index(string searchString)
        {
            var profiles = _context.Profiles.Include(profile => profile.BugUser).ToList();

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                profiles = profiles.Where(i => i.OwnerName.Contains(searchString)).ToList();
            }
            
            var profileVMs = new List<ProfileViewModel>();
            foreach (var profile in profiles)
            {
                var convertedProfile = profile.ConvertToProfileVM();
                profileVMs.Add(convertedProfile);
            }

            return View( profileVMs);
        }

        // GET: Profiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .Include(p => p.BugUser).ThenInclude(p => p.IssueReportEntities)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (profile == null)
            {
                return NotFound();
            }

            var convertedProfile = profile.ConvertToProfileVM();

            return View(convertedProfile);
        }

        // GET: Profiles/Details/5
        public async Task<IActionResult> MyProfile(string? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .Include(p => p.BugUser).ThenInclude(p => p.IssueReportEntities)
                .FirstOrDefaultAsync(m => m.BugUserId == id);

            if (profile == null)
            {
                return NotFound();
            }

            var convertedProfile = profile.ConvertToProfileVM();

            return View(convertedProfile);
        }

        // GET: Profiles/Create
        public IActionResult Create()
        {
            ViewData["BugUserId"] = new SelectList(_context.BugUser, "Id", "Id");
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,BugUserId")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BugUserId"] = new SelectList(_context.BugUser, "Id", "Id", profile.BugUserId);
            return View(profile);
        }

        // GET: Profiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            ViewData["BugUserId"] = new SelectList(_context.BugUser, "Id", "Id", profile.BugUserId);
            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,BugUserId")] Profile profile)
        {
            if (id != profile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.Id))
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
            ViewData["BugUserId"] = new SelectList(_context.BugUser, "Id", "Id", profile.BugUserId);
            return View(profile);
        }

        // GET: Profiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .Include(p => p.BugUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profiles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Profiles'  is null.");
            }
            var profile = await _context.Profiles.FindAsync(id);
            if (profile != null)
            {
                _context.Profiles.Remove(profile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(int id)
        {
          return _context.Profiles.Any(e => e.Id == id);
        }
    }
}
