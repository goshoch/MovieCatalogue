using MovieCatalog.Data.Entities;
using System.Threading.Tasks;

namespace MovieCatalogue.Core.Interfaces
{
    public interface IUserReviewService
    {
        Task<bool> UserExistsAsync(int userId);
        Task<bool> MovieExistsAsync(int movieId);
        Task<bool> ReviewExistsAsync(int userId, int movieId);
        Task<UserReview?> GetReviewAsync(int userId, int movieId);
        Task CreateAsync(UserReview review);
        Task UpdateRatingAsync(int userId, int movieId, double rating);
        Task DeleteAsync(int userId, int movieId);
    }
}
