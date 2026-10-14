using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Data.Entities;
using MovieCatalog.Data;
using MovieCatalogue.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogue.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(u =>
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search));
            }

            var userList = await users
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new UserListViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ReviewsCount = u.UserReviews.Count
                })
                .ToListAsync();

            ViewBag.Search = search;

            return View(userList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDetailsViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,

                    Reviews = u.UserReviews.Select(r => new UserReview
                    {
                        UserId = r.UserId,
                        MovieId = r.MovieId,
                        Rating = r.Rating,
                        Movie = r.Movie
                    }).ToList(),

                    AllMovies = _context.Movies
                        .OrderBy(m => m.Title)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return View(user);
        }

        public IActionResult Create()
        {
            return View(new UserCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            var model = new UserEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, UserEditViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDeleteViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ReviewsCount = u.UserReviews.Count
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserReviews)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            if (user.UserReviews.Any())
            {
                TempData["Error"] =
                    "Cannot delete a user with existing reviews. " +
                    "Delete their reviews first.";

                return RedirectToAction(nameof(Delete), new { id });
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

