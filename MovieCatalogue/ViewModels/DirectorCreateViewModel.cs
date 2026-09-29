using System.ComponentModel.DataAnnotations;

namespace MovieCatalogue.ViewModels
{
    public class DirectorCreateViewModel
    {
        [Required]
        [StringLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Nationality { get; set; }

        [StringLength(500)]
        public string Biography { get; set; }
    }
}