using System;
using System.Collections.Generic;

namespace OnlineLibrary.Models.DBObjects
{
    public partial class Genre
    {
        public Genre()
        {
            Books = new HashSet<Book>();
        }

        public Guid Idgenre { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
