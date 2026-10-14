using MovieCatalog.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieCatalogue.ViewModels
{
    public class UserDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public List<UserReview> Reviews { get; set; } = new();

        public List<Movie> AllMovies { get; set; } = new();

    }
}
