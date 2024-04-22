using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public int? AccountId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? FullAddress { get; set; }
    public string? FullName { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public virtual Account? Account { get; set; }
}
