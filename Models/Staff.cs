using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Staff
    {
        public Staff()
        {
            //Order = new HashSet<Order>();
        }

        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string OptionalData { get; set; }
        public DateTime? AddedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        //public virtual ICollection<Order> Order { get; set; }
    }
}
