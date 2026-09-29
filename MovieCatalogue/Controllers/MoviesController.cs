using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieCatalog.Data.Entities;
using MovieCatalog.Data;
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.ViewModels;

namespace MovieCatalogue.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var movies = _context.Movies.Include(m => m.Director).AsQueryable();
            
            if (!string.IsNullOrEmpty(search))
            {
                movies = movies.Where(m => m.Title.Contains(search) 
                    || m.Director.FirstName.Contains(search) 
                    || m.Director.LastName.Contains(search)
                    || m.Genre.Contains(search));
            }

            var movieList = await movies
                .OrderBy(m => m.Title)
                .Select(m => new MovieListViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = $"{m.Director.FirstName} {m.Director.LastName}",
                    Genre = m.Genre,
                    ReleaseYear = m.ReleaseYear,
                    Rating = m.Rating,
                    Language = m.Language
                })
                .ToListAsync();

            ViewBag.Search = search;
            return View(movieList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Director)
                .Where(m => m.Id == id)
                .Select(m => new MovieDetailsViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = $"{m.Director.FirstName} {m.Director.LastName}",
                    Genre = m.Genre,
                    ReleaseYear = m.ReleaseYear,
                    Duration = m.Duration,
                    Rating = m.Rating,
                    Language = m.Language,
                    Description = m.Description
                })
                .FirstOrDefaultAsync();

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        public async Task<IActionResult> Create()
        {
            var model = new MovieCreateViewModel
            {
                Directors = await _context.Directors
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = $"{d.FirstName} {d.LastName}"
                    })
                    .ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Directors = await _context.Directors
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = $"{d.FirstName} {d.LastName}"
                    })
                    .ToListAsync();
                return View(model);
            }

            var movie = new Movie
            {
                Title = model.Title,
                DirectorId = model.DirectorId,
                Genre = model.Genre,
                ReleaseYear = model.ReleaseYear,
                Duration = model.Duration,
                Rating = model.Rating,
                Language = model.Language,
                Description = model.Description
            };
            _context.Add(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            var model = new MovieEditViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                DirectorId = movie.DirectorId,
                Genre = movie.Genre,
                ReleaseYear = movie.ReleaseYear,
                Duration = movie.Duration,
                Rating = movie.Rating,
                Language = movie.Language,
                Description = movie.Description,
                Directors = await _context.Directors
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = $"{d.FirstName} {d.LastName}"
                    })
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MovieEditViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.Directors = await _context.Directors
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = $"{d.FirstName} {d.LastName}"
                    })
                    .ToListAsync();
                return View(model);
            }

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            movie.Title = model.Title;
            movie.DirectorId = model.DirectorId;
            movie.Genre = model.Genre;
            movie.ReleaseYear = model.ReleaseYear;
            movie.Duration = model.Duration;
            movie.Rating = model.Rating;
            movie.Language = model.Language;
            movie.Description = model.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Director)
                .Where(m => m.Id == id)
                .Select(m => new MovieDeleteViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = $"{m.Director.FirstName} {m.Director.LastName}",
                    Genre = m.Genre,
                    Rating = m.Rating,
                })
                .FirstOrDefaultAsync();

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}