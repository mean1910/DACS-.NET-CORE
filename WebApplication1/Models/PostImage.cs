using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class PostImage
{
    public int ImageId { get; set; }

    public string? Url { get; set; }

    public int? PostId { get; set; }

    public virtual Post? Post { get; set; }
}
