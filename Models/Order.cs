using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItem = new HashSet<OrderItem>();
            SalesTransaction = new HashSet<SalesTransaction>();
        }

        public int OrderId { get; set; }
        public string OrderInvoice { get; set; }
        public int IsAdjustment { get; set; }
        public int IsGst { get; set; }
        public int CustomerId { get; set; }
        public int StaffId { get; set; }
        public float Amount { get; set; }
        public float Gstpercent { get; set; }
        public float? DiscountPercent { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public DateTime? AddedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
        public virtual ICollection<SalesTransaction> SalesTransaction { get; set; }
    }
}
