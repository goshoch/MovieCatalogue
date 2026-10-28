using MovieCatalog.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCatalogue.Core.Interfaces
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAllAsync(string? search = null);
        Task<Movie?> GetByIdAsync(int id);
        Task<Movie?> GetDetailsAsync(int id);
        Task<List<Director>> GetDirectorsAsync();
        Task CreateAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task DeleteAsync(int id);
    }
}
