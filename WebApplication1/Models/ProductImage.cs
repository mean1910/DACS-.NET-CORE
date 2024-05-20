using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class ProductImage
{
    public int ImageId { get; set; }

    public string Url { get; set; } = null!;

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
