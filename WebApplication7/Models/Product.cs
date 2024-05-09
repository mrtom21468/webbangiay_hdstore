using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Models;

public partial class Product
{
    public int ProductId { get; set; }
    [DisplayName("Tên sản phẩm")]
    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm.")]
    public string ProductName { get; set; } = null!;

    public bool? State { get; set; }
    [DisplayName("Thương hiệu")]

    public int? BrandId { get; set; }
    [DisplayName("Thể loại")]

    public int? CategoryId { get; set; }

    public string? ImagePath { get; set; }

    [MaxLength(500)]
    [DisplayName("Mô tả")]
    public string? Description { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    // Thêm thuộc tính để lưu trữ file ảnh
    [NotMapped] // Đánh dấu không ánh xạ vào cơ sở dữ liệu
    [DisplayName("Upload Image")]
    [Required(ErrorMessage = "Vui lòng tải lên ảnh sản phẩm.")]
    public IFormFile ImageFile { get; set; }
}
