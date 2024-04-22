﻿using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
