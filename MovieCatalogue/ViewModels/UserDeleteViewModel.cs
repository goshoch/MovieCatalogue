using System.ComponentModel.DataAnnotations;

namespace MovieCatalogue.ViewModels
{
    public class UserDeleteViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public int ReviewsCount { get; set; }
    }
}
