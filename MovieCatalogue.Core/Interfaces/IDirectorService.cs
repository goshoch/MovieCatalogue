using MovieCatalog.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCatalogue.Core.Interfaces
{
    public interface IDirectorService
    {
        Task<List<Director>> GetAllAsync(string? search = null);
        Task<Director?> GetByIdAsync(int id);
        Task<Director?> GetDetailsAsync(int id);
        Task CreateAsync(Director director);
        Task UpdateAsync(Director director);
        Task<bool> DeleteAsync(int id);
    }
}
