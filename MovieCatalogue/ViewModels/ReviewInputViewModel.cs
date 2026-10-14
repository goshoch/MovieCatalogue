using System.ComponentModel.DataAnnotations;

namespace MovieCatalogue.ViewModels
{
    public class ReviewInputViewModel
    {
        public int UserId { get; set; }

        public int MovieId { get; set; }

        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }
    }
}
