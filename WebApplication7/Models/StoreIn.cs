using System;
using System.Collections.Generic;
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
        public int StoreInId { get; set; }
        public int? ProductdetailId { get; set; }
        public int? AccountId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime CreatedAt { get; set; }

        public decimal ImprortPrice { get; set; }
        public int Quantity { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual ProductDetail? ProductDetail { get; set; }
        public virtual Account? Account { get; set; }
    }
}
