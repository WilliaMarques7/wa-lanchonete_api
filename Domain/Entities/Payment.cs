using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Payment
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? InStoreOrderId { get; set; }

    public string? QrData { get; set; }

    public int? PaymentStatus { get; set; }

    public virtual Order? Order { get; set; }
}
