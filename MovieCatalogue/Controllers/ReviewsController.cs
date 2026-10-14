using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Data.Entities;
using MovieCatalog.Data;
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.ViewModels;

namespace MovieCatalogue.Controllers
{
    public class ReviewsController : Controller
    {
            private readonly ApplicationDbContext _context;

            public ReviewsController(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Create(
                int userId, int movieId)
            {
                var userExists = await _context.Users
                    .AnyAsync(u => u.Id == userId);

                var movieExists = await _context.Movies
                    .AnyAsync(m => m.Id == movieId);

                if (!userExists || !movieExists)
                    return NotFound();

                var exists = await _context.UserReviews
                    .AnyAsync(r => r.UserId == userId
                                && r.MovieId == movieId);

                if (exists)
                    return Conflict();

                return View("Create", new ReviewInputViewModel
                {
                    UserId = userId,
                    MovieId = movieId,
                    Rating = 1
                });
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(
                ReviewInputViewModel model)
            {
                if (!ModelState.IsValid)
                    return View("Create", model);

                var userExists = await _context.Users
                    .AnyAsync(u => u.Id == model.UserId);

                var movieExists = await _context.Movies
                    .AnyAsync(m => m.Id == model.MovieId);

                if (!userExists || !movieExists)
                    return NotFound();

                var exists = await _context.UserReviews
                    .AnyAsync(r => r.UserId == model.UserId
                                && r.MovieId == model.MovieId);

                if (exists)
                    return Conflict();

                var review = new UserReview
                {
                    UserId = model.UserId,
                    MovieId = model.MovieId,
                    Rating = model.Rating
                };

                _context.UserReviews.Add(review);

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    "Details", "Users",
                    new { id = model.UserId });
            }

            public async Task<IActionResult> Edit(
                int userId, int movieId)
            {
                var review = await _context.UserReviews
                    .FirstOrDefaultAsync(r =>
                        r.UserId == userId &&
                        r.MovieId == movieId);

                if (review == null)
                    return NotFound();

                var model = new ReviewInputViewModel
                {
                    UserId = review.UserId,
                    MovieId = review.MovieId,
                    Rating = review.Rating
                };

                return View("Edit", model);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(
                ReviewInputViewModel model)
            {
                if (!ModelState.IsValid)
                    return View("Edit", model);

                var review = await _context.UserReviews
                    .FirstOrDefaultAsync(r =>
                        r.UserId == model.UserId &&
                        r.MovieId == model.MovieId);

                if (review == null)
                    return NotFound();

                review.Rating = model.Rating;

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    "Details", "Users",
                    new { id = review.UserId });
            }

            public async Task<IActionResult> Delete(
                int userId, int movieId)
            {
                var review = await _context.UserReviews
                    .Include(r => r.Movie)
                    .FirstOrDefaultAsync(r =>
                        r.UserId == userId &&
                        r.MovieId == movieId);

                if (review == null)
                    return NotFound();

                return View("Delete", review);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(
                int userId, int movieId)
            {
                var review = await _context.UserReviews
                    .FirstOrDefaultAsync(r =>
                        r.UserId == userId &&
                        r.MovieId == movieId);

                if (review == null)
                    return NotFound();

                _context.UserReviews.Remove(review);

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    "Details", "Users",
                    new { id = userId });
            }
        }
    }

