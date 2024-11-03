using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiorello.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Desc { get; set; }
        public string Sign { get; set; }
        [Required]
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
