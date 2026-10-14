using System.ComponentModel.DataAnnotations;

namespace MovieCatalogue.ViewModels
{
    public class DirectorDeleteViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Nationality { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Movies Count")]
        public int MoviesCount { get; set; }
    }
}