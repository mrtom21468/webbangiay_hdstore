using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Models;

public partial class Order
{
    [DisplayName("Mã đơn hàng")]
    public int OrderId { get; set; }

    public int? AccountId { get; set; }

    [DisplayName("Địa chỉ")]

    public string? Address { get; set; }
    [StringLength(30)]
    [DisplayName("Họ và tên")]

    public string? FullName { get; set; }
    [DisplayName("Số điện thoại")]

    public string? PhoneNumber { get; set; }

    [DisplayName("Trạng thái thanh toán")]
    public string? PaymentStatus { get; set; }
    [DisplayName("Phương thức thanh toán")]

    public string? PaymentType { get; set; }

    [DisplayName("Trạng thái đơn hàng")]

    public string? Status { get; set; }

    [DisplayName("Tổng tiền")]

    public decimal? TotalAmount { get; set; }

    [StringLength(64)]
    public string? OrderIdMoMo { get; set; }
    [StringLength(64)]
    public string? ReqrIdMoMo { get; set; }

    [DisplayName("Ngày tạo")]

    public DateTime? CreatedAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
