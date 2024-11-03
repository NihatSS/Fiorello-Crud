using System.ComponentModel.DataAnnotations.Schema;

namespace Fiorello.Models
{
    public class SliderImage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public List<IFormFile> Photo { get; set; }
    }
}
