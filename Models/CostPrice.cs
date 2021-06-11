using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class CostPrice
    {
        public int CostPriceId { get; set; }
        public string ItemName { get; set; }
        public int StoreId { get; set; }
        public int Quantityinstore { get; set; }
        public DateTime? AddedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual CurrentStock CurrentStock { get; set; }
    }
}
