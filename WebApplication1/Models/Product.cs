using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ShortDesc { get; set; }

    public string Description { get; set; } = null!;

    public int? CatId { get; set; }

    public int? Price { get; set; }

    public int? Discount { get; set; }

    public string? Thumb { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateModified { get; set; }

    public bool? BestSeller { get; set; }

    public bool? HomeFlag { get; set; }

    public bool? Active { get; set; }

    public string? Tags { get; set; }

    public int? UnitsInStock { get; set; }

    public virtual Category? Cat { get; set; }
}
