using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Data.Entities;
using MovieCatalogue.Core.Interfaces;
using MovieCatalogue.ViewModels;

namespace MovieCatalogue.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(string search)
        {
            var users = await _userService.GetAllAsync(search);

            var userList = users.Select(u => new UserListViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ReviewsCount = u.UserReviews?.Count ?? 0
            }).ToList();

            ViewBag.Search = search;

            return View(userList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _userService.GetByIdAsync(id.Value);

            if (user == null)
                return NotFound();

            var allMovies = await _userService.GetAllMoviesAsync();

            var model = new UserDetailsViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Reviews = user.UserReviews?.Select(r => new UserReview
                {
                    UserId = r.UserId,
                    MovieId = r.MovieId,
                    Rating = r.Rating,
                    Movie = r.Movie
                }).ToList() ?? new List<UserReview>(),
                AllMovies = allMovies
            };

            return View(model);
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

            await _userService.CreateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _userService.GetByIdAsync(id.Value);

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

            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await _userService.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _userService.GetByIdAsync(id.Value);

            if (user == null)
                return NotFound();

            var model = new UserDeleteViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ReviewsCount = user.UserReviews?.Count ?? 0
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var canDelete = await _userService.DeleteAsync(id);

            if (!canDelete)
            {
                TempData["Error"] =
                    "Cannot delete a user with existing reviews. " +
                    "Delete their reviews first.";

                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
