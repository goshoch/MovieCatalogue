using MovieCatalog.Data;
using MovieCatalog.Data.Entities;
using MovieCatalogue.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCatalogue.Core.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly ApplicationDbContext _context;

        public DirectorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Director>> GetAllAsync(string? search = null)
        {
            var query = _context.Directors.Include(d => d.Movies).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d =>
                    d.FirstName.Contains(search) ||
                    d.LastName.Contains(search) ||
                    d.Nationality.Contains(search));
            }

            return await query.OrderBy(d => d.LastName).ThenBy(d => d.FirstName).ToListAsync();
        }

        public async Task<Director?> GetByIdAsync(int id) =>
            await _context.Directors.FindAsync(id);

        public async Task<Director?> GetDetailsAsync(int id) =>
            await _context.Directors.Include(d => d.Movies).FirstOrDefaultAsync(d => d.Id == id);

        public async Task CreateAsync(Director director)
        {
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Director director)
        {
            _context.Directors.Update(director);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var director = await _context.Directors.Include(d => d.Movies).FirstOrDefaultAsync(d => d.Id == id);
            if (director == null) return false;
            if (director.Movies.Any()) return false;

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
