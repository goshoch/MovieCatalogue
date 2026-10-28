using MovieCatalog.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCatalogue.Core.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync(string? search = null);
        Task<User?> GetByIdAsync(int id);
        Task<List<Movie>> GetAllMoviesAsync();
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
    }
}
