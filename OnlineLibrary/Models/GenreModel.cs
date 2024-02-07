using OnlineLibrary.Models.DBObjects;

namespace OnlineLibrary.Models
{
    public class GenreModel
    {
        public Guid Idgenre { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
        public decimal TotalFees { get; internal set; }
        public int TotalCopies { get; internal set; }
    }
}
