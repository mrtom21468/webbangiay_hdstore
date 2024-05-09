using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using WebApplication7.Areas.Admin.Helpper;
using Microsoft.AspNetCore.Hosting;
using AspNetCoreHero.ToastNotification.Notyf;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly QLDBcontext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly NotyfService _NotyfService;
        public CategoriesController(QLDBcontext context, IWebHostEnvironment webHostEnvironment, NotyfService NotyfService)    
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _NotyfService = NotyfService;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }


        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,ImageFile")] Category category)
        {
            if (ModelState.IsValid)
            {
                
                category.CategorySlug = Slug.GenerateSlug(category.CategoryName);
                var checkslug=_context.Categories.FirstOrDefault(c => c.CategorySlug == category.CategorySlug);
                if (checkslug == null)
                {
                    if (category.ImageFile != null && category.ImageFile.Length > 0)
                    {
                        // Tạo tên cho ảnh sản phẩm
                        string nameProductImage = Guid.NewGuid().ToString() + Path.GetExtension(category.ImageFile.FileName);

                        // Đường dẫn đến thư mục lưu trữ ảnh
                        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Userdata", "img", "categories");

                        // Đường dẫn đầy đủ tới tệp ảnh
                        string imagePath = Path.Combine(uploadFolder, nameProductImage);

                        // Lưu ảnh vào thư mục tạm thời
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await category.ImageFile.CopyToAsync(stream);
                        }

                        // Lưu đường dẫn của ảnh vào model
                        category.ImagePath = "/Userdata/img/categories/" + nameProductImage;
                        _context.Add(category);
                        await _context.SaveChangesAsync();
                        _NotyfService.Success("Thêm danh mục sản phẩm thành công", 3);
                        return RedirectToAction("Index");
                    }
                    _NotyfService.Error("Vui lòng tải tệp lên", 3);
                    return View(category);
                }
                _NotyfService.Error("Tên danh mục bị trùng", 3);
                return View(category);

            }
            _NotyfService.Error("Dư liệu không hợp lệ", 3);
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,ImageFile,ImagePath")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                category.CategorySlug = Slug.GenerateSlug(category.CategoryName);
                var checkslug = _context.Categories.Where(c => c.CategorySlug == category.CategorySlug && c.CategoryId!=id).ToList();
                if(checkslug.Count()>0)
                {
                    _NotyfService.Error("Trùng tên danh mục", 3);
                    return View(category);
                }
                if (category.ImageFile != null && category.ImageFile.Length > 0)
                    {
                        string fileExtension = Path.GetExtension(category.ImageFile.FileName).ToLower();
                        if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
                        {
                            // Tạo tên cho ảnh sản phẩm
                            string nameProductImage = Guid.NewGuid().ToString() + Path.GetExtension(category.ImageFile.FileName);

                            // Đường dẫn đến thư mục lưu trữ ảnh
                            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Userdata", "img", "categories");

                            // Đường dẫn đầy đủ tới tệp ảnh
                            string imagePath = Path.Combine(uploadFolder, nameProductImage);

                            // Lưu ảnh vào thư mục tạm thời
                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                await category.ImageFile.CopyToAsync(stream);
                            }

                            // Lưu đường dẫn của ảnh vào model
                            category.ImagePath = "/Userdata/img/categories/" + nameProductImage;
                        }
                        else
                        {
                            _NotyfService.Error("Dư liệu ảnh tải lên không hợp lệ", 3);
                            return View(category);
                        }
                    }
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    _NotyfService.Success("Chỉnh sửa danh mục sản phẩm thành công", 3);
                return RedirectToAction(nameof(Index));
            }
            _NotyfService.Error("Dư liệu không hợp lệ hoặc trùng tên danh mục", 3);
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                }

                await _context.SaveChangesAsync();
                _NotyfService.Success("Xóa danh mục sản phẩm thành công", 3);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) { 

                    _NotyfService.Error("Xóa danh mục sản phẩm thất bại", 3);
                return RedirectToAction(nameof(Index));
            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
