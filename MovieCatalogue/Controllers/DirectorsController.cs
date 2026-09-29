using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Data.Entities;
using MovieCatalog.Data;
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.ViewModels;

namespace MovieCatalogue.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DirectorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var directors = _context.Directors.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                directors = directors.Where(d => d.FirstName.Contains(search) || d.LastName.Contains(search) || d.Nationality.Contains(search));
            }

            var directorList = await directors
                .OrderBy(d => d.LastName).ThenBy(d => d.FirstName)
                .Select(d => new DirectorListViewModel
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Nationality = d.Nationality,
                    BirthDate = d.BirthDate,
                    MoviesCount = d.Movies.Count
                })
                .ToListAsync();

            ViewBag.Search = search;
            return View(directorList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var director = await _context.Directors
                .Include(d => d.Movies)
                .Where(d => d.Id == id)
                .Select(d => new DirectorDetailsViewModel
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    BirthDate = d.BirthDate,
                    Nationality = d.Nationality,
                    Biography = d.Biography,
                    Movies = d.Movies.Select(m => new MovieListViewModel
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Director = $"{d.FirstName} {d.LastName}",
                        Genre = m.Genre,
                        ReleaseYear = m.ReleaseYear,
                        Duration = m.Duration,
                        Rating = m.Rating,
                        Language = m.Language
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (director == null)
                return NotFound();

            return View(director);
        }

        public IActionResult Create()
        {
            return View(new DirectorCreateViewModel { BirthDate = DateTime.Today.AddYears(-30) });
        }

        [HttpPost]
        public async Task<IActionResult> Create(DirectorCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var director = new Director
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Nationality = model.Nationality,
                Biography = model.Biography
            };

            _context.Add(director);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var director = await _context.Directors.FindAsync(id);

            if (director == null)
                return NotFound();

            var model = new DirectorEditViewModel
            {
                Id = director.Id,
                FirstName = director.FirstName,
                LastName = director.LastName,
                BirthDate = director.BirthDate,
                Nationality = director.Nationality,
                Biography = director.Biography
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DirectorEditViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var director = await _context.Directors.FindAsync(id);

            if (director == null)
                return NotFound();

            director.FirstName = model.FirstName;
            director.LastName = model.LastName;
            director.BirthDate = model.BirthDate;
            director.Nationality = model.Nationality;
            director.Biography = model.Biography;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var director = await _context.Directors
                .Where(d => d.Id == id)
                .Select(d => new DirectorDeleteViewModel
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Nationality = d.Nationality,
                    BirthDate = d.BirthDate,
                    MoviesCount = d.Movies.Count
                })
                .FirstOrDefaultAsync();

            if (director == null)
                return NotFound();

            return View(director);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var director = await _context.Directors
                .Include(d => d.Movies)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (director == null)
            {
                return NotFound();
            }

            if (director.Movies.Any())
            {
                TempData["Error"] = "Cannot delete director with existing movies. Please delete or reassign movies first.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}