using WebApplication7.Models;

namespace WebApplication7.ViewModel
{
    public class TopProductsViewModel
    {
        public Product Products { get; set; }

        public ProductDetail ProductDetails { get; set; }
        public int? TotalQuantity { get; set; }
    }
}
