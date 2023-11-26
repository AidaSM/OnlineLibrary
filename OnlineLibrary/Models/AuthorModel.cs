using System.ComponentModel.DataAnnotations;
using OnlineLibrary.Models.DBObjects;

namespace OnlineLibrary.Models
{
    public class AuthorModel
    {
        public Guid Idauthor { get; set; }
        public string Name { get; set; } = null!;
        [DisplayFormat(DataFormatString = "0:MM/dd/yyyy")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string? Nationality { get; set; }

        public virtual ICollection<Book> Books { get; set; }


    }
}
