using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Items
    {
        public Items()
        {
            //OrderItem = new HashSet<OrderItem>();
            //PurchaseItem = new HashSet<PurchaseItem>();
        }

        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public int? Checker { get; set; }
        public string CategoryId { get; set; }
        public string BrandId { get; set; }
        public string Description { get; set; }
        public float? Gstpercent { get; set; }
        public float? CostPrice { get; set; }
        public float? SellingPrice { get; set; }
        public float? DiscountPercent { get; set; }
        public string Hsncode { get; set; }
        public string AliasCode { get; set; }
        public string WarehouseId { get; set; }
        //public int? Quantity { get; set; }
        public string ImageUrl { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        //public virtual ICollection<OrderItem> OrderItem { get; set; }
        //public virtual ICollection<PurchaseItem> PurchaseItem { get; set; }
    }
}
