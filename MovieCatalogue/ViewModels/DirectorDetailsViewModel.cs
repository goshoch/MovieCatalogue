using System.ComponentModel.DataAnnotations;

namespace MovieCatalogue.ViewModels
{
    public class DirectorDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        public string Nationality { get; set; }

        public string Biography { get; set; }

        public List<MovieListViewModel> Movies { get; set; } = new List<MovieListViewModel>();
    }
}