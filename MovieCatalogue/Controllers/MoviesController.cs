using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieCatalog.Data.Entities;
using MovieCatalogue.Core.Interfaces;
using MovieCatalogue.ViewModels;

namespace MovieCatalogue.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Index(string search)
        {
            var movies = await _movieService.GetAllAsync(search);

            var movieList = movies.Select(m => new MovieListViewModel
            {
                Id = m.Id,
                Title = m.Title,
                Director = $"{m.Director?.FirstName ?? string.Empty} {m.Director?.LastName ?? string.Empty}".Trim(),
                Genre = m.Genre,
                ReleaseYear = m.ReleaseYear,
                Rating = m.Rating,
                Language = m.Language
            }).ToList();

            ViewBag.Search = search;
            return View(movieList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _movieService.GetDetailsAsync(id.Value);

            if (movie == null)
                return NotFound();

            var model = new MovieDetailsViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Director = $"{movie.Director?.FirstName ?? string.Empty} {movie.Director?.LastName ?? string.Empty}".Trim(),
                Genre = movie.Genre,
                ReleaseYear = movie.ReleaseYear,
                Duration = movie.Duration,
                Rating = movie.Rating,
                Language = movie.Language,
                Description = movie.Description
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new MovieCreateViewModel
            {
                Directors = await GetDirectorsSelectList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Directors = await GetDirectorsSelectList();
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

            await _movieService.CreateAsync(movie);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _movieService.GetByIdAsync(id.Value);

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
                Directors = await GetDirectorsSelectList()
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
                model.Directors = await GetDirectorsSelectList();
                return View(model);
            }

            var movie = await _movieService.GetByIdAsync(id);

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

            await _movieService.UpdateAsync(movie);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _movieService.GetByIdAsync(id.Value);

            if (movie == null)
                return NotFound();

            var model = new MovieDeleteViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Director = $"{movie.Director?.FirstName ?? string.Empty} {movie.Director?.LastName ?? string.Empty}".Trim(),
                Genre = movie.Genre,
                Rating = movie.Rating,
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movieService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetDirectorsSelectList()
        {
            var directors = await _movieService.GetDirectorsAsync();
            return directors.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.FirstName} {d.LastName}"
            });
        }
    }
}
