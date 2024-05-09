using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Models;

public partial class Size
{
    public int SizeId { get; set; }
    [DisplayName("Tên size")]
    [Required(ErrorMessage ="Vui lòng nhập size")]
    public string SizeName { get; set; } = null!;

    public string? SizeSlug { get; set; } = null!;

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
