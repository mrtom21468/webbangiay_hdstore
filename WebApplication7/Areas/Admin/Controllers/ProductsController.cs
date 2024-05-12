using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using Microsoft.AspNetCore.Hosting;
using AspNetCoreHero.ToastNotification.Notyf;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductsController : Controller
    {
        private readonly QLDBcontext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly NotyfService _notyfService;
        public ProductsController(QLDBcontext context, IWebHostEnvironment webHostEnvironment, NotyfService notyfService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _notyfService=notyfService;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var qLDBcontext = _context.Products.Include(p => p.Brand).Include(p => p.Category).Include(p=>p.ProductDetails);
            return View(await qLDBcontext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Manager(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products
              .Include(p => p.Brand)
              .Include(p => p.Category)
              .Include(p => p.ProductDetails)
                  .ThenInclude(pd => pd.Size)
              .Include(p => p.ProductDetails)
                  .ThenInclude(pd => pd.Color)
              .FirstOrDefaultAsync(m => m.ProductId == id);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName");
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
               [HttpPost]
        public async Task<IActionResult> Search(int? id,string? keyword)
        {
            var productDetails = _context.ProductDetails
                                                    .Include(p => p.Color)
                                                    .Include(p => p.Size)
                                                    .Where(p => p.ProductId == id);

            if (!string.IsNullOrEmpty(keyword))
            {
                var keywords = keyword.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var key in keywords)
                {
                    string keywordLower = key.ToLower(); // Chuyển key về dạng không phân biệt hoa thường

                    productDetails = productDetails.Where(p => p.Color.ColorName.ToLower().Contains(keywordLower)
                                            || p.Size.SizeName.ToLower().Contains(keywordLower)
                                            || p.Quantity.ToString().Contains(key)
                                            || p.SellingPrice.ToString().Contains(key)
                                            || p.PurchasePrice.ToString().Contains(key));
                }
            }
            return PartialView("_SearchResultsProductDetail", productDetails);
        }
        //thêm mới phiên bản sản phẩm
        [HttpPut]
        public async Task<IActionResult> Create([FromBody] ProductDetail model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //kiểm tra xem có trùng không
                    var checkproduct = _context.ProductDetails.Where(p => p.ProductId == model.ProductId && p.ColorId == model.ColorId && p.SizeId == model.SizeId).FirstOrDefault();
                    if (checkproduct != null)
                    {
                        return Ok(new { status = false, mes = "Thêm thất bại do đã tồn tại phiên bản của sản phẩm" });
                    }
                    // Tạo một đối tượng Product từ dữ liệu được gửi từ biểu mẫu
                    var ProductDetail = new ProductDetail
                    {
                        ProductId = model.ProductId,
                        SizeId = model.SizeId,
                        ColorId = model.ColorId,
                        Quantity = model.Quantity,
                        SellingPrice = model.SellingPrice,
                        PurchasePrice = model.PurchasePrice
                    };

                    // Thực hiện các thao tác lưu vào cơ sở dữ liệu
                    _context.ProductDetails.Add(ProductDetail);
                    _context.SaveChanges();
                    var productDetails = _context.ProductDetails
                                            .Include(p => p.Color)
                                            .Include(p => p.Size)
                                            .Where(p => p.ProductId == model.ProductId);
                    // Trả về một phản hồi thành công (ví dụ: mã thành công 200) kèm theo một thông điệp
                    return PartialView("_SearchResultsProductDetail", productDetails);
                }
                else
                {
                    // Nếu dữ liệu không hợp lệ, trả về một phản hồi lỗi với các thông báo lỗi
                    return Ok(new { status = false, mes = "Thêm thất bại do dữ liệu không hợp lệ" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, mes = "Thêm thất bại lỗi hệ thống" });

            }
        }
        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,BrandId,CategoryId,Description,ImageFile")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    // Tạo tên cho ảnh sản phẩm
                    string nameProductImage = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageFile.FileName);

                    // Đường dẫn đến thư mục lưu trữ ảnh
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Userdata", "img", "product");

                    // Đường dẫn đầy đủ tới tệp ảnh
                    string imagePath = Path.Combine(uploadFolder, nameProductImage);

                    // Lưu ảnh vào thư mục tạm thời
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn của ảnh vào model
                    product.ImagePath = "/Userdata/img/product/" + nameProductImage;
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm sản phẩm thành công", 3);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            _notyfService.Warning("Thêm sản phẩm tthất bại", 3);

            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,BrandId,CategoryId,Description,ImageFile")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (product.ImageFile != null && product.ImageFile.Length > 0)
                    {
                        // Tạo tên cho ảnh sản phẩm
                        string nameProductImage = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageFile.FileName);

                        // Đường dẫn đến thư mục lưu trữ ảnh
                        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Userdata", "img", "product");

                        // Đường dẫn đầy đủ tới tệp ảnh
                        string imagePath = Path.Combine(uploadFolder, nameProductImage);

                        // Lưu ảnh vào thư mục tạm thời
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await product.ImageFile.CopyToAsync(stream);
                        }

                        // Lưu đường dẫn của ảnh vào model
                        product.ImagePath = "/Userdata/img/product/" + nameProductImage;
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // Action để cập nhật trạng thái
        [HttpPost]
        public ActionResult UpdateState(int productId, bool newState)
        {
            try
            {
                // Thực hiện logic để cập nhật trạng thái trong cơ sở dữ liệu hoặc nơi khác
                _context.Products.FirstOrDefault(p => p.ProductId == productId).State = newState;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
