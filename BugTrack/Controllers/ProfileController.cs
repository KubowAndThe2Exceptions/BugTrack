using BugTrack.Areas.Identity.Data;
using BugTrack.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BugTrack.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<BugUser> _userManager;
        private readonly UserContext _userContext;

        public ProfileController(UserManager<BugUser> userManager, UserContext userContext)
        {
            _userManager = userManager;
            _userContext = userContext;
        }


        // GET: ProfileController
        public async Task<IActionResult> Index()
        {
            var allUsers = from u in _userContext.Users
                           select u;

            return View(await allUsers.ToListAsync());
        }

        // GET: ProfileController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _userContext.Users == null)
            {
                return NotFound();
            }

            var targetUser = from u in _userContext.Users
                             where u.Id == id.ToString()
                             select u;

            if (targetUser == null)
            {
                return NotFound();
            }

            return View(targetUser);
        }

        // GET: ProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProfileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
