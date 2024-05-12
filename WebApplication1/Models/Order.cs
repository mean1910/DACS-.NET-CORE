using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public int? StransacStatusId { get; set; }

    public bool? Deleted { get; set; }

    public bool? Paid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int? PaymentId { get; set; }

    public string Note { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual TransactStatus? StransacStatus { get; set; }
}
