using MovieCatalog.Data;
using MovieCatalog.Data.Entities;
using MovieCatalogue.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MovieCatalogue.Core.Services
{
    public class UserReviewService : IUserReviewService
    {
        private readonly ApplicationDbContext _context;

        public UserReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExistsAsync(int userId) =>
            await _context.Users.AnyAsync(u => u.Id == userId);

        public async Task<bool> MovieExistsAsync(int movieId) =>
            await _context.Movies.AnyAsync(m => m.Id == movieId);

        public async Task<bool> ReviewExistsAsync(int userId, int movieId) =>
            await _context.UserReviews.AnyAsync(r => r.UserId == userId && r.MovieId == movieId);

        public async Task<UserReview?> GetReviewAsync(int userId, int movieId) =>
            await _context.UserReviews
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);

        public async Task CreateAsync(UserReview review)
        {
            _context.UserReviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRatingAsync(int userId, int movieId, double rating)
        {
            var review = await _context.UserReviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);
            if (review != null)
            {
                review.Rating = rating;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int userId, int movieId)
        {
            var review = await _context.UserReviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);
            if (review != null)
            {
                _context.UserReviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}
