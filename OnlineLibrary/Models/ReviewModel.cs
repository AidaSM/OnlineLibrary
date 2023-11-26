using OnlineLibrary.Models.DBObjects;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class ReviewModel
    {
        public Guid Idreview { get; set; }
        public Guid Idmember { get; set; }
        public Guid Idbook { get; set; }
        public decimal Rating { get; set; }
        public string? Text { get; set; }
        [DisplayFormat(DataFormatString = "0:MM/dd/yyyy")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public virtual Book IdbookNavigation { get; set; } = null!;
        public virtual Member IdmemberNavigation { get; set; } = null!;

    }
}
