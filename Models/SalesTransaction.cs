using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class SalesTransaction
    {
        public int TransactionId { get; set; }
        public int OrderId { get; set; }
        public int AmountPaid { get; set; }
        public DateTime? AddedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual Order Order { get; set; }
    }
}
