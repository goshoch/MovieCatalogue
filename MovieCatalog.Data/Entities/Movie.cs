using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalog.Data.Entities
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [ForeignKey("Director")]
        public int DirectorId { get; set; }

        public Director Director { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; }

        [Required]
        [Range(1900, 2026)]
        public int ReleaseYear { get; set; }

        [Required]
        [Range(1, 500)]
        public int Duration { get; set; }

        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }

        [Required]
        public string Language { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public ICollection<UserReview> UserReviews { get; set; }

    }
}