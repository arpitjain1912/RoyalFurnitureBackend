using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Brand
    {
        public Brand()
        {
            //Items = new HashSet<Items>();
        }

        public string BrandId { get; set; }
        public string BrandName { get; set; }

        //public virtual ICollection<Items> Items { get; set; }
    }
}
