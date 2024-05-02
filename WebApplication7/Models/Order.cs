using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? AccountId { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? PaymentStatus { get; set; }
    public string? PaymentType { get; set; }

    public string? Status { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
