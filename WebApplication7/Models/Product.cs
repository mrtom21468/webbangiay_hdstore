using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int? BrandId { get; set; }

    public int? CategoryId { get; set; }

    public int? SupplierId { get; set; }

    public decimal? PurchasePrice { get; set; }

    public string? ImagePath { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    public virtual Supplier? Supplier { get; set; }
    // Thêm thuộc tính để lưu trữ file ảnh
    [NotMapped] // Đánh dấu không ánh xạ vào cơ sở dữ liệu
    [DisplayName("Upload Image")]
    public IFormFile ImageFile { get; set; }
}
