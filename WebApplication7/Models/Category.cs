using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WebApplication7.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? CategorySlug { get; set; }
    public string? ImagePath { get; set; }
    // Thêm thuộc tính để lưu trữ file ảnh
    [NotMapped] // Đánh dấu không ánh xạ vào cơ sở dữ liệu
    [DisplayName("Upload Image")]
    public IFormFile? ImageFile { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
