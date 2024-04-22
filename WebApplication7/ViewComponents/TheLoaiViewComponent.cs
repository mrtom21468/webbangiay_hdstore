using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Controllers;
using WebApplication7.Models;

namespace WebApplication7.ViewComponents
{
    [ViewComponent(Name = "TheLoai")]
    public class TheLoaiViewComponent: ViewComponent
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QLDBcontext _QLDBcontext;
        public TheLoaiViewComponent(ILogger<HomeController> logger, QLDBcontext QLDBcontext)
        {
            _logger = logger;
            _QLDBcontext = QLDBcontext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Logic đơn giản: tạo danh sách các mục
            var items =await _QLDBcontext.Categories.ToListAsync();
            return View(items);
        }
    }
}
