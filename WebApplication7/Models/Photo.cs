using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class Photo
{
    public int PhotoId { get; set; }

    public string? PhotoName { get; set; }

    public string PhotoUrl { get; set; } = null!;

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
