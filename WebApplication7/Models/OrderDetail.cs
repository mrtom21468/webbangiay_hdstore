using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductdetailId { get; set; }
    public decimal? PriceAtOrderTime { get; set; } // Thêm trường giá thời điểm đặt
    public int? Quantity { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ProductDetail? Productdetail { get; set; }
}
