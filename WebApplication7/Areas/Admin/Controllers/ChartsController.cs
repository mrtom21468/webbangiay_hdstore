using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChartsController : Controller
    {
        private readonly QLDBcontext _context;
        public ChartsController(QLDBcontext context)
        {
               _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetChartData()
        {
            List<DateTime> recentMonths = new List<DateTime>();

            // Lấy tháng hiện tại
            DateTime currentMonth = DateTime.Now;

            // Thêm tháng hiện tại vào danh sách
            recentMonths.Add(currentMonth);

            // Thêm 11 tháng trước đó vào danh sách
            for (int i = 1; i <= 11; i++)
            {
                DateTime previousMonth = currentMonth.AddMonths(-i);
                recentMonths.Add(previousMonth);
            }
            var threerecentMonthsLabels = recentMonths.Take(3).Select(dt => dt.ToString("MM/yyyy")).ToArray();
            var sixrecentMonthsLabels = recentMonths.Take(6).Select(dt => dt.ToString("MM/yyyy")).ToArray();
            var recentMonthsLabels = recentMonths.Take(12).Select(dt => dt.ToString("MM/yyyy")).ToArray();
            var allrecentMonthsLabels = recentMonths.Take(15).Select(dt => dt.ToString("MM/yyyy")).ToArray();
            var listbrand= _context.Brands.ToList();
            foreach (var i in listbrand)
            {
                
            }
            //lấy dữ liệu của các hãng vào một mảng 
            var salesData = new
            {
                threeMonths = new
                {
                    labels = threerecentMonthsLabels.Reverse(),
                    nike = new[] { 1000, 1500, 2000 },
                    adidas = new[] { 1200, 1700, 1800 },
                    puma = new[] { 900, 1400, 1500 }
                },
                sixMonths = new
                {
                    labels = sixrecentMonthsLabels.Reverse(),
                    nike = new[] { 1000, 1500, 2000, 1800, 2200, 2500 },
                    adidas = new[] { 1200, 1700, 1800, 1600, 2100, 2300 },
                    puma = new[] { 900, 1400, 1500, 1300, 1700, 1900 }
                },
                oneYear = new
                {
                    labels = recentMonthsLabels.Reverse(),
                    nike = new[] { 1000, 1500, 2000, 1800, 2200, 2500, 2200, 2000, 1800, 2100, 2400, 2700 },
                    adidas = new[] { 1200, 1700, 1800, 1600, 2100, 2300, 2000, 1900, 1800, 1900, 2300, 2500 },
                    puma = new[] { 900, 1400, 1500, 1300, 1700, 1900, 1600, 1500, 1400, 1500, 1700, 1900 }
                },
                all = new
                {
                    labels = allrecentMonthsLabels.Reverse(),
                    nike = new[] { 1000, 1500, 2000, 1800, 2200, 2500, 2200, 2000, 1800, 2100, 2400, 2700 },
                    adidas = new[] { 1200, 1700, 1800, 1600, 2100, 2300, 2000, 1900, 1800, 1900, 2300, 2500 },
                    puma = new[] { 900, 1400, 1500, 1300, 1700, 1900, 1600, 1500, 1400, 1500, 1700, 1900 }
                }
            };

            return Json(salesData);
        }
    }
}
