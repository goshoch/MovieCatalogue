using System.ComponentModel.DataAnnotations;

namespace MovieCatalogue.ViewModels
{
    public class UserEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
