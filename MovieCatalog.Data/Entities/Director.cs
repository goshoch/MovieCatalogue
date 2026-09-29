using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalog.Data.Entities
{
    public class Director
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(30)]
        public string LastName { get; set; }
        [Required]

        public DateTime BirthDate { get; set; }
        [Required]

        public string Nationality { get; set; }
        [StringLength(500)]

        public string Biography { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
