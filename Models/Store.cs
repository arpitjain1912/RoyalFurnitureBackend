using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Store
    {
        public Store()
        {
            //OrderItem = new HashSet<OrderItem>();
            //PurchaseItem = new HashSet<PurchaseItem>();
        }

        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime? AddedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        //public virtual ICollection<OrderItem> OrderItem { get; set; }
        //public virtual ICollection<PurchaseItem> PurchaseItem { get; set; }
    }
}
