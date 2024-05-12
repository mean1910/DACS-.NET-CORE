using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TransactStatus
{
    public int TransactStatusId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
