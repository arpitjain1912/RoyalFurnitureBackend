using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class OrderItem
    {
        public OrderItem()
        {
            CostPriceAssigned = new HashSet<CostPriceAssigned>();
        }

        public string ItemName { get; set; }
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public int Quantity { get; set; }
        public int? CostPrice { get; set; }
        public int SellingPrice { get; set; }
        public DateTime? AddedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual CurrentStock CurrentStock { get; set; }
        public virtual Items ItemNameNavigation { get; set; }
        public virtual Order Order { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<CostPriceAssigned> CostPriceAssigned { get; set; }
    }
}
