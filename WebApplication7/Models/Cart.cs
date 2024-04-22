using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class Cart
{
    public int ProductdetailId { get; set; }

    public int AccountId { get; set; }

    public int Amount { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ProductDetail Productdetail { get; set; } = null!;
}
