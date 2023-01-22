using System;
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
using BugTrack.ViewModels.VMIssueReportEntities;

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
            //Should contain issue report view models instead of issuereport itself
            var issues = from i in _context.IssueReport.Include(p => p.BugUser).ThenInclude(p => p.Profile)
                         select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                issues = from i in _context.IssueReport
                         where i.IssueTitle.Contains(searchString)
                         select i;
            }
            var issueList = issues.ToList();
            var issueVMlist = new List<IssueReportEntityWithIdViewModel>();
            foreach (var issue in issueList)
            {
                var convertedIssue = issue.ConvertToIssueReportEntityWithIdVM();
                issueVMlist.Add(convertedIssue);
            }

            return View(issueVMlist);
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
            await _context.Entry(issueReportEntity).Collection(p => p.Comments).LoadAsync();
            await _context.Entry(issueReportEntity).Reference(p => p.BugUser).LoadAsync();

            var issueReportVM = issueReportEntity.ConvertToIssueReportEntityWithIdVM();

            return View(issueReportVM);
        }
        [HttpPost]
        public async Task<IActionResult> Details(int? id, string commentBody)
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
            await _context.Entry(issueReportEntity).Collection(p => p.Comments).LoadAsync();
            Comment newComment = new Comment(commentBody, DateTime.Now);
            newComment.BugUser = await _userManager.GetUserAsync(User);
            newComment.OwnerName = newComment.BugUser.FirstName + " " + newComment.BugUser.LastName;
            issueReportEntity.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            var issueReportVM = issueReportEntity.ConvertToIssueReportEntityWithIdVM();

            return View(issueReportVM);
        }

        // GET: IssueReports/Create
        public IActionResult Create()
        {
            var model = new IssueReportEntityEditCreateVM();
            return View(model);
        }

        // POST: IssueReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IssueThreat,IssueStatus,ModuleOrClass,GeneralDescription,ReplicationDescription,IssueTitle,DateFound")] IssueReportEntityEditCreateVM issueReportEntityViewModel)
        {
            if (ModelState.IsValid)
            {
                var bugUser = await _userManager.GetUserAsync(User);
                var issueReportEntity = issueReportEntityViewModel.ConvertToIssueReportEntity(bugUser);

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
            var issueReportEntityVM = issueReportEntity.ConvertToIssueReportEntityWithIdVM();


            return View(issueReportEntityVM);
        }

        // POST: IssueReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IssueThreat,IssueStatus,ModuleOrClass,GeneralDescription,ReplicationDescription,IssueTitle,DateFound")] IssueReportEntityEditCreateVM issueReportEntityVM)
        {
            if (id != issueReportEntityVM.Id)
            {
                return NotFound();
            }

            var issueReportEntity = await _context.IssueReport.FindAsync(issueReportEntityVM.Id);
            if (issueReportEntity == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(issueReportEntity).Reference(i => i.BugUser).Load();
                    _context.Entry(issueReportEntity).State = EntityState.Detached;

                    issueReportEntity = issueReportEntityVM.ConvertToIssueReportEntity(issueReportEntity.BugUser);
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
            return View(issueReportEntityVM);
        }

        // GET: IssueReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //Needs Refactor
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

        public async Task<IActionResult> MyIssues()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userDB = await _context.BugUser.Where(p => p.Id == currentUser.Id).FirstOrDefaultAsync();
            _context.Entry(userDB).Collection(p => p.IssueReportEntities).Load();

            var issueList = userDB.IssueReportEntities.ToList();
            var convertedList = new List<IssueReportEntityWithIdViewModel>();
            foreach (var issue in issueList)
            {
                var convertedIssue = issue.ConvertToIssueReportEntityWithIdVM();
                convertedList.Add(convertedIssue);
            }

            return View(convertedList);
        }
    }
}
