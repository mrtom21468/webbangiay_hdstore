using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    [DisplayName("Tên thương hiệu")]
    [Required(ErrorMessage ="Vui lòng nhập tên thương hiệu")]
    public string BrandName { get; set; } = null!;
    public string? BrandSlug { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
