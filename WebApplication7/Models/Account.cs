using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication7.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? WalletId { get; set; }

    public string UserId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<StoreIn> StoreIns { get; set; } = new List<StoreIn>();
}
