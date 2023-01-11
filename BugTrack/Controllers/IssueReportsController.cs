﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrack.Data;
using BugTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BugTrack.Areas.Identity.Data;
using BugTrack.ViewModels;

namespace BugTrack.Controllers
{
    [Authorize]
    public class IssueReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BugUser> _userManager;

        public IssueReportsController(ApplicationDbContext context, UserManager<BugUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: IssueReports
        public async Task<IActionResult> Index(string searchString)
        {
            var issues = from i in _context.IssueReport
                         select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                issues = from i in _context.IssueReport
                         where i.IssueTitle.Contains(searchString)
                         select i;
            }

            return View(await issues.ToListAsync());
        }

        // GET: IssueReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.IssueReport == null)
            {
                return NotFound();
            }

            var issueReportEntity = await _context.IssueReport
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issueReportEntity == null)
            {
                return NotFound();
            }

            return View(issueReportEntity);
        }

        // GET: IssueReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IssueReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ThreatLevel,GeneralDescription,ReplicationDescription,IssueTitle,DateFound")] IssueReportEntityViewModel issueReportEntityViewModel)
        {
            if (ModelState.IsValid)
            {
                var bugUser = await _userManager.GetUserAsync(User);
                var issueReportEntity = new IssueReportEntity();
                issueReportEntity.IssueTitle = issueReportEntityViewModel.IssueTitle;
                issueReportEntity.ThreatLevel = issueReportEntityViewModel.ThreatLevel;
                issueReportEntity.GeneralDescription = issueReportEntityViewModel.GeneralDescription;
                issueReportEntity.ReplicationDescription = issueReportEntityViewModel.ReplicationDescription;
                issueReportEntity.DateFound = issueReportEntityViewModel.DateFound;
                issueReportEntity.BugUser = bugUser;

                _context.Add(issueReportEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(issueReportEntityViewModel);
        }

        // GET: IssueReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.IssueReport == null)
            {
                return NotFound();
            }

            var issueReportEntity = await _context.IssueReport.FindAsync(id);
            if (issueReportEntity == null)
            {
                return NotFound();
            }
            return View(issueReportEntity);
        }

        // POST: IssueReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ThreatLevel,GeneralDescription,ReplicationDescription,IssueTitle,DateFound")] IssueReportEntity issueReportEntity)
        {
            if (id != issueReportEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(issueReportEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueReportEntityExists(issueReportEntity.Id))
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
            return View(issueReportEntity);
        }

        // GET: IssueReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.IssueReport == null)
            {
                return NotFound();
            }

            var issueReportEntity = await _context.IssueReport
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issueReportEntity == null)
            {
                return NotFound();
            }

            return View(issueReportEntity);
        }

        // POST: IssueReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.IssueReport == null)
            {
                return Problem("Entity set 'ApplicationDbContext.IssueReport'  is null.");
            }
            var issueReportEntity = await _context.IssueReport.FindAsync(id);
            if (issueReportEntity != null)
            {
                _context.IssueReport.Remove(issueReportEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueReportEntityExists(int id)
        {
          return _context.IssueReport.Any(e => e.Id == id);
        }
    }
}
