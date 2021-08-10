using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class CurrentStock
    {
        public CurrentStock()
        {
            CostPrice = new HashSet<CostPrice>();
            //OrderItem = new HashSet<OrderItem>();
            //PurchaseItem = new HashSet<PurchaseItem>();
        }

        public string ItemName { get; set; }
        public int StoreId { get; set; }
        public int CostPriceId { get; set; }
        public int TotalQuantityInStore { get; set; }
        public int TotalQuantityLeft { get; set; }
        public DateTime? AddedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<CostPrice> CostPrice { get; set; }
        //public virtual ICollection<OrderItem> OrderItem { get; set; }
        //public virtual ICollection<PurchaseItem> PurchaseItem { get; set; }
    }
}
