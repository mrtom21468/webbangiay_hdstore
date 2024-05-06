using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Controllers;
using WebApplication7.Models;
using WebApplication7.ViewModel;

namespace WebApplication7.ViewComponents
{
    [ViewComponent(Name = "SanPhamBanChay")]
    public class SanPhamBanChayViewComponent : ViewComponent
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QLDBcontext _QLDBcontext;
        public SanPhamBanChayViewComponent(ILogger<HomeController> logger, QLDBcontext QLDBcontext)
        {
            _logger = logger;
            _QLDBcontext = QLDBcontext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topProducts =_QLDBcontext.OrderDetails
                .Include(od => od.Productdetail)
                    .ThenInclude(pd => pd.Product)
                .GroupBy(od => od.Productdetail.ProductId)
                .Select(g => new TopProductsViewModel
                {
                    Products = g.FirstOrDefault().Productdetail.Product, // Lấy ra sản phẩm từ bất kỳ chi tiết đơn hàng trong nhóm
                    ProductDetails= g.Select(od => od.Productdetail).FirstOrDefault(),
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(g => g.TotalQuantity)
                .Take(8)
                .ToList();

            return View(topProducts);
        }
    }
}
