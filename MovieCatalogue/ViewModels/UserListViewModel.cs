using System.ComponentModel.DataAnnotations;

namespace MovieCatalogue.ViewModels
{
    public class UserListViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Movies Watched")]
        public int ReviewsCount { get; set; }
    }
}
