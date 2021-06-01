using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class DispatchReady
    {
        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public string OrderId { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
