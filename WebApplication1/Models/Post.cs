using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string? Title { get; set; }

    public string? ShortContents { get; set; }

    public string? Contents { get; set; }

    public string? Thumb { get; set; }

    public bool? Published { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Author { get; set; }

    public int? AccountId { get; set; }

    public string? Tags { get; set; }
}
