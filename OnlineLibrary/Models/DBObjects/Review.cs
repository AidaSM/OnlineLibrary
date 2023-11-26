using System;
using System.Collections.Generic;

namespace OnlineLibrary.Models.DBObjects
{
    public partial class Review
    {
        public Guid Idreview { get; set; }
        public Guid Idmember { get; set; }
        public Guid Idbook { get; set; }
        public decimal Rating { get; set; }
        public string? Text { get; set; }
        public DateTime? Date { get; set; }

        public virtual Book IdbookNavigation { get; set; } = null!;
        public virtual Member IdmemberNavigation { get; set; } = null!;
    }
}
