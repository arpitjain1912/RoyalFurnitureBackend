using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Vendor
    {
        public Vendor()
        {
            //Purchase = new HashSet<Purchase>();
        }

        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gstnumber { get; set; }

        //public virtual ICollection<Purchase> Purchase { get; set; }
    }
}
