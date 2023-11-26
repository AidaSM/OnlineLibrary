using OnlineLibrary.Models.DBObjects;

namespace OnlineLibrary.Models
{
    public class BookModel
    {
        public Guid Idbook { get; set; }
        public string Title { get; set; } = null!;
        public Guid Idauthor { get; set; }
        public string? Isbn { get; set; }
        public int? PublicationYear { get; set; }
        public Guid? Idgenre { get; set; }
        public string? Language { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string? ImagePath { get; set; }
        public virtual Author IdauthorNavigation { get; set; } = null!;
        public virtual Genre? IdgenreNavigation { get; set; }
        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public AuthorModel Author { get; internal set; }
    }
}
