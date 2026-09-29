using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieCatalogue.ViewModels
{
    public class MovieCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Director")]
        public int DirectorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; }

        [Range(1900, 2100)]
        public int ReleaseYear { get; set; }

        [Range(1, 500)]
        public int Duration { get; set; }

        [Range(1, 10)]
        public double Rating { get; set; }

        [Required]
        public string Language { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public IEnumerable<SelectListItem> Directors { get; set; } = new List<SelectListItem>();
    }
}