using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication7.Models
{
    public partial class StoreIn
    {
        public StoreIn()
        {
            CreatedAt = DateTime.Now; // Đặt giá trị mặc định cho CreatedAt khi khởi tạo đối tượng StoreIn
        }
        [DisplayName("Mã đơn")]
        public int StoreInId { get; set; }
        [DisplayName("Thông tin sản phẩm")]
        public int? ProductdetailId { get; set; }
        public int? AccountId { get; set; }
        [DisplayName("Nhà cung cấp")]
        public int? SupplierId { get; set; }
        [DisplayName("Ngày tạo")]

        public DateTime CreatedAt { get; set; }
        [DisplayName("Giá nhập")]
        public decimal ImprortPrice { get; set; }
        [DisplayName("Số lượng nhập")]

        public int Quantity { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual ProductDetail? ProductDetail { get; set; }
        public virtual Account? Account { get; set; }
    }
}
