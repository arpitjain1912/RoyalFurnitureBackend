using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Purchase
    {
        public Purchase()
        {
            PurchaseItem = new HashSet<PurchaseItem>();
        }

        public int PurchaseId { get; set; }
        public string PurchaseInvoice { get; set; }
        public int IsGst { get; set; }
        public int VendorId { get; set; }
        public float? Amount { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int? Status { get; set; }
        public DateTime? AddedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItem { get; set; }
    }
}
