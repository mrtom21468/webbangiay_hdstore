using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class ProductDetail
{
    public int ProductdetailId { get; set; }

    public int? ProductId { get; set; }

    public int? SizeId { get; set; }

    public int? ColorId { get; set; }

    public int? Quantity { get; set; }

    public decimal? SellingPrice { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Color? Color { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Product? Product { get; set; }

    public virtual Size? Size { get; set; }
}
