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
            //OrderItem = new HashSet<OrderItem>();
            //PurchaseItem = new HashSet<PurchaseItem>();
        }
        //ItemName not made foreign key here just use simple query and join.
        public string ItemName { get; set; }
        public int StoreId { get; set; }
        public int ItemId { get; set; }
        public int QuantityInStore { get; set; }
        public int QuantityLeft { get; set; }
        public DateTime LastUpdate { get; set; }

        //public virtual ICollection<OrderItem> OrderItem { get; set; }
        //public virtual ICollection<PurchaseItem> PurchaseItem { get; set; }
    }
}
