using System;
using System.Collections.Generic;

namespace OnlineLibrary.Models.DBObjects
{
    public partial class Transaction
    {
        public Guid Idtransaction { get; set; }
        public string Idmember { get; set; }
        public Guid Idbook { get; set; }
        public DateTime Date { get; set; }
        public DateTime Retrun { get; set; }
        public string? Status { get; set; }

        public virtual Book IdbookNavigation { get; set; } = null!;
        public virtual Member IdmemberNavigation { get; set; } = null!;
    }
}
