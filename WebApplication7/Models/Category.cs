using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    [DisplayName("Tên danh mục")]
    [Required(ErrorMessage ="Vui lòng nhập tên danh mục")]
    public string CategoryName { get; set; } = null!;

    public string? CategorySlug { get; set; }
    public string? ImagePath { get; set; }
    // Thêm thuộc tính để lưu trữ file ảnh
    [NotMapped] // Đánh dấu không ánh xạ vào cơ sở dữ liệu
    [DisplayName("Tải ảnh danh mục")]
    public IFormFile? ImageFile { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
