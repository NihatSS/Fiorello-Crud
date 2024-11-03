using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiorello.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; } 
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductImage> Images { get; set; }
        [NotMapped]
        public List<IFormFile> UploadedImages { get; set; }
    }
}
