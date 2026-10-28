using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Data.Entities;
using MovieCatalogue.Core.Interfaces;
using MovieCatalogue.ViewModels;

namespace MovieCatalogue.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IUserReviewService _reviewService;

        public ReviewsController(IUserReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<IActionResult> Create(
            int userId, int movieId)
        {
            var userExists = await _reviewService.UserExistsAsync(userId);
            var movieExists = await _reviewService.MovieExistsAsync(movieId);

            if (!userExists || !movieExists)
                return NotFound();

            var exists = await _reviewService.ReviewExistsAsync(userId, movieId);

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

            var userExists = await _reviewService.UserExistsAsync(model.UserId);
            var movieExists = await _reviewService.MovieExistsAsync(model.MovieId);

            if (!userExists || !movieExists)
                return NotFound();

            var exists = await _reviewService.ReviewExistsAsync(model.UserId, model.MovieId);

            if (exists)
                return Conflict();

            var review = new UserReview
            {
                UserId = model.UserId,
                MovieId = model.MovieId,
                Rating = model.Rating
            };

            await _reviewService.CreateAsync(review);

            return RedirectToAction(
                "Details", "Users",
                new { id = model.UserId });
        }

        public async Task<IActionResult> Edit(
            int userId, int movieId)
        {
            var review = await _reviewService.GetReviewAsync(userId, movieId);

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

            var review = await _reviewService.GetReviewAsync(model.UserId, model.MovieId);

            if (review == null)
                return NotFound();

            await _reviewService.UpdateRatingAsync(model.UserId, model.MovieId, model.Rating);

            return RedirectToAction(
                "Details", "Users",
                new { id = review.UserId });
        }

        public async Task<IActionResult> Delete(
            int userId, int movieId)
        {
            var review = await _reviewService.GetReviewAsync(userId, movieId);

            if (review == null)
                return NotFound();

            return View("Delete", review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int userId, int movieId)
        {
            var review = await _reviewService.GetReviewAsync(userId, movieId);

            if (review == null)
                return NotFound();

            await _reviewService.DeleteAsync(userId, movieId);

            return RedirectToAction(
                "Details", "Users",
                new { id = userId });
        }
    }
}
