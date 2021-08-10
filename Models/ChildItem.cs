using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class ChildItem
    {
        public string ItemName { get; set; }
        public string ChildItemName { get; set; }
        public int NumberOfCopy { get; set; }
        public DateTime? AddedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual Items ItemNameNavigation { get; set; }
    }
}
