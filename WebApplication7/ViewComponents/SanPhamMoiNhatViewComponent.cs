using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Controllers;
using WebApplication7.Models;

namespace WebApplication7.ViewComponents
{
    [ViewComponent(Name = "SanPhamMoiNhat")]
    public class SanPhamMoiNhatViewComponent : ViewComponent
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QLDBcontext _QLDBcontext;
        public SanPhamMoiNhatViewComponent(ILogger<HomeController> logger, QLDBcontext QLDBcontext)
        {
            _logger = logger;
            _QLDBcontext = QLDBcontext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _QLDBcontext.Products
                .Include(p => p.ProductDetails)
                .Where(p => p.ProductDetails.Any(pd => pd.Quantity > 0) && p.State==true)
                .OrderByDescending(p => p.ProductId) // Sắp xếp theo thời gian thêm mới nhất
                .Take(8)
                .ToList();
            return View(items);
        }
    }
}
