using System;
using System.Collections.Generic;

namespace OnlineLibrary.Models.DBObjects
{
    public partial class Fee
    {
        public Guid Idfee { get; set; }
        public Guid Idbook { get; set; }
        public Guid Idmember { get; set; }
        public string? Description { get; set; }
        public int IsPaid { get; set; }
        public decimal Amount { get; set; }

        public virtual Book IdbookNavigation { get; set; } = null!;
        public virtual Member IdmemberNavigation { get; set; } = null!;
    }
}
