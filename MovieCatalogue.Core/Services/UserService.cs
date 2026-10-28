using MovieCatalog.Data;
using MovieCatalog.Data.Entities;
using MovieCatalogue.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCatalogue.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync(string? search = null)
        {
            var query = _context.Users.Include(u => u.UserReviews).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search));
            }

            return await query.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users
                .Include(u => u.UserReviews)
                .ThenInclude(ur => ur.Movie)
                .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<List<Movie>> GetAllMoviesAsync() =>
            await _context.Movies.OrderBy(m => m.Title).ToListAsync();

        public async Task CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.Include(u => u.UserReviews).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return false;
            if (user.UserReviews.Any()) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
