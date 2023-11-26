using System;
using System.Collections.Generic;

namespace OnlineLibrary.Models.DBObjects
{
    public partial class Book
    {
        public Book()
        {
            Fees = new HashSet<Fee>();
            Reviews = new HashSet<Review>();
            Transactions = new HashSet<Transaction>();
        }

        public Guid Idbook { get; set; }
        public string Title { get; set; } = null!;
        public Guid Idauthor { get; set; }
        public string? Isbn { get; set; }
        public int? PublicationYear { get; set; }
        public Guid? Idgenre { get; set; }
        public string? Language { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public virtual Author IdauthorNavigation { get; set; } = null!;
        public virtual Genre? IdgenreNavigation { get; set; }
        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
