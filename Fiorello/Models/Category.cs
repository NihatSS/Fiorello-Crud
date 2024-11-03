using System.ComponentModel.DataAnnotations;

namespace Fiorello.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
