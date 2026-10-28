using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Data.Entities;
using MovieCatalogue.Core.Interfaces;
using MovieCatalogue.ViewModels;

namespace MovieCatalogue.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly IDirectorService _directorService;

        public DirectorsController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        public async Task<IActionResult> Index(string search)
        {
            var directors = await _directorService.GetAllAsync(search);

            var directorList = directors.Select(d => new DirectorListViewModel
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Nationality = d.Nationality,
                BirthDate = d.BirthDate,
                MoviesCount = d.Movies?.Count ?? 0
            }).ToList();

            ViewBag.Search = search;
            return View(directorList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var director = await _directorService.GetDetailsAsync(id.Value);

            if (director == null)
                return NotFound();

            var model = new DirectorDetailsViewModel
            {
                Id = director.Id,
                FirstName = director.FirstName,
                LastName = director.LastName,
                BirthDate = director.BirthDate,
                Nationality = director.Nationality,
                Biography = director.Biography,
                Movies = director.Movies?.Select(m => new MovieListViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = $"{m.Director?.FirstName ?? string.Empty} {m.Director?.LastName ?? string.Empty}".Trim(),
                    Genre = m.Genre,
                    ReleaseYear = m.ReleaseYear,
                    Duration = m.Duration,
                    Rating = m.Rating,
                    Language = m.Language
                }).ToList() ?? new List<MovieListViewModel>()
            };

            return View(model);
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

            await _directorService.CreateAsync(director);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var director = await _directorService.GetByIdAsync(id.Value);

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
            {
                return View(model);
            }

            var director = await _directorService.GetByIdAsync(id);

            if (director == null)
                return NotFound();

            director.FirstName = model.FirstName;
            director.LastName = model.LastName;
            director.BirthDate = model.BirthDate;
            director.Nationality = model.Nationality;
            director.Biography = model.Biography;

            await _directorService.UpdateAsync(director);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var director = await _directorService.GetByIdAsync(id.Value);

            if (director == null)
                return NotFound();

            var model = new DirectorDeleteViewModel
            {
                Id = director.Id,
                FirstName = director.FirstName,
                LastName = director.LastName,
                Nationality = director.Nationality,
                BirthDate = director.BirthDate,
                MoviesCount = director.Movies?.Count ?? 0
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var canDelete = await _directorService.DeleteAsync(id);

            if (!canDelete)
            {
                TempData["Error"] = "Cannot delete director with existing movies. Please delete or reassign movies first.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
