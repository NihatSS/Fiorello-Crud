namespace Fiorello.ViewModels.Admin.Product
{
    using Fiorello.Models;
    public class ProductEditVM
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}
