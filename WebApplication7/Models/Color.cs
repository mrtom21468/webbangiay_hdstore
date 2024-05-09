using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Models;

public partial class Color
{
    public int ColorId { get; set; }

    [DisplayName("Tên màu sắc")]
    [Required(ErrorMessage ="Vui lòng nhập tên màu sắc")]
    public string ColorName { get; set; } = null!;
    public string? ColorSlug { get; set; } = null!;

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
