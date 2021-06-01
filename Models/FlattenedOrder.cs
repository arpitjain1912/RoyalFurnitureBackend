using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class FlattenedOrder
    {
        public string OrderId { get; set; }
        public string InvoiceId { get; set; }
        public string CustomerId { get; set; }
        public float Amount { get; set; }
        public float Gstpercent { get; set; }
        public float? Cgst { get; set; }
        public float? Sgst { get; set; }
        public float? Igst { get; set; }
        public float? DiscountPercent { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public string Items { get; set; }
        public string Remarks { get; set; }
        public float AdvancePaid { get; set; }
    }
}
