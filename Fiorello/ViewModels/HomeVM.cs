using Fiorello.Models;

namespace Fiorello.ViewModels
{
    public class HomeVM
    {
        public Slider Slider { get; set; }
        public List<SliderImage> SliderImages { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
