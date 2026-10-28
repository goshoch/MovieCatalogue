using MovieCatalog.Data;
using MovieCatalog.Data.Entities;
using MovieCatalogue.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCatalogue.Core.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetAllAsync(string? search = null)
        {
            var query = _context.Movies.Include(m => m.Director).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m =>
                    m.Title.Contains(search) ||
                    m.Director.FirstName.Contains(search) ||
                    m.Director.LastName.Contains(search) ||
                    m.Genre.Contains(search));
            }

            return await query.OrderBy(m => m.Title).ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id) =>
            await _context.Movies.FindAsync(id);

        public async Task<Movie?> GetDetailsAsync(int id) =>
            await _context.Movies.Include(m => m.Director).FirstOrDefaultAsync(m => m.Id == id);

        public async Task<List<Director>> GetDirectorsAsync() =>
            await _context.Directors.OrderBy(d => d.LastName).ThenBy(d => d.FirstName).ToListAsync();

        public async Task CreateAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }
    }
}
