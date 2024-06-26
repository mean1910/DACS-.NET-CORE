﻿using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FullName { get; set; }

    public DateTime? BirthDay { get; set; }

    public string? Avatar { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime? DateCreate { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
