using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Controllers;
using WebApplication7.Models;

namespace WebApplication7.ViewComponents
{
    [ViewComponent(Name = "ThuongHieu")]
    public class ThuongHieuViewComponent: ViewComponent
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QLDBcontext _QLDBcontext;
        public ThuongHieuViewComponent(ILogger<HomeController> logger, QLDBcontext QLDBcontext)
        {
            _logger = logger;
            _QLDBcontext = QLDBcontext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Logic đơn giản: tạo danh sách các mục
            var items =await _QLDBcontext.Brands.ToListAsync();
            return View(items);
        }
    }
}
